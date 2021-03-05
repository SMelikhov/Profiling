using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Profiling.Common.InstanceData
{
	internal class PerformanceCounters 
	{
		private readonly bool _isClientUi;
		private readonly List<PerformanceCounterItem> _counters = new List<PerformanceCounterItem>();
		private int _exceptionCount;
		private const int MaxExceptionCount = 100;

		private volatile bool _initialized;

		public PerformanceCounters(bool isClientUi)
		{
			_isClientUi = isClientUi;
		}

		public Dictionary<string, float> GetStatistics()
		{
			try
			{
				EnsureInitialized();
				lock (_counters)
				{
					return _counters.ToDictionary(counter => counter.Name, counter => counter.Read());
				}
			}
			catch (UnauthorizedAccessException)
			{
				ResetCounterForUnauthorizedIfRequered();
			}
			catch (Exception)
			{
				//do nothing by design
			}

			return new Dictionary<string, float>();
		}

		private void ResetCounterForUnauthorizedIfRequered()
		{
			_exceptionCount++;

			if (_exceptionCount > MaxExceptionCount)
			{
				lock (_counters)
				{
					//we do not collect counters in case we do not have permissions
					_counters.Clear();
					_initialized = true;
				}
			}
		}

		public static string GetProcessInstanceName(bool isClientUi)
		{
			using (var process = Process.GetCurrentProcess())
			{
				if (!isClientUi)
					return process.ProcessName;

				var category = new PerformanceCounterCategory("Process");

				var instances = category.GetInstanceNames();
				foreach (var instance in instances)
				{
					using (var cnt = new PerformanceCounter("Process", "ID Process", instance, true))
					{
						var val = (int) cnt.RawValue;
						if (val == process.Id)
						{
							return instance;
						}
					}
				}

				return process.ProcessName;
			}
		}

		private void EnsureInitialized()
		{
			if (!_initialized)
			{
				lock (_counters)
				{
					if (!_initialized)
					{
						try
						{
							_counters.Clear();
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter("Processor", "% Processor Time", "_Total", true), "TotalCpu"));

							string processName = GetProcessInstanceName(_isClientUi);

							_counters.Add(new PerformanceCounterItem(new PerformanceCounter("Process", "% Processor Time", processName, true), "ProcessCpu"));

							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR LocksAndThreads", "Contention Rate / sec", processName, true), "ContentionRateSec"));

							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps", processName, true), "BytesInAllHeaps"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR Memory", "# Gen 0 Collections", processName, true), "Gen0Collections"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR Memory", "# Gen 1 Collections", processName, true), "Gen1Collections"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR Memory", "# Gen 2 Collections", processName, true), "Gen2Collections"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR Memory", "% Time in GC", processName, true), "TimeInGc"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR Memory", "Allocated Bytes/sec", processName, true), "AllocatedBytesSec"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR Memory", "Gen 0 heap size", processName, true), "Gen0HeapSize"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR Memory", "Gen 1 heap size", processName, true), "Gen1HeapSize"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR Memory", "Gen 2 heap size", processName, true), "Gen2HeapSize"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter(".NET CLR Memory", "Large Object Heap size", processName, true), "LargeObjectHeapSize"));



							var physicalDiskCategory = new PerformanceCounterCategory("PhysicalDisk");
							var physicalDiskCategoryNames = physicalDiskCategory.GetInstanceNames();

							foreach (var name in physicalDiskCategoryNames)
							{
								string itemName;
								if (name == "_Total")
									itemName = string.Empty;
								else
								{
									var disks = name.Split(new [] {" "}, StringSplitOptions.RemoveEmptyEntries);
									itemName = disks.Last();
								}

								_counters.Add(new PerformanceCounterItem(new PerformanceCounter("PhysicalDisk", "Avg. Disk sec/Write", name, true), "DiskWriteSec" + itemName));
								_counters.Add(new PerformanceCounterItem(new PerformanceCounter("PhysicalDisk", "Avg. Disk sec/Read", name, true), "DiskReadSec" + itemName));
								_counters.Add(new PerformanceCounterItem(new PerformanceCounter("PhysicalDisk", "Avg. Disk Queue Length", name, true), "AvgDiskQueueLength" + itemName));
								_counters.Add(new PerformanceCounterItem(new PerformanceCounter("PhysicalDisk", "Avg. Disk Bytes/Read", name, true), "DiskReadAvgSize" + itemName));
								_counters.Add(new PerformanceCounterItem(new PerformanceCounter("PhysicalDisk", "Avg. Disk Bytes/Write", name, true), "DiskWriteAvgSize" + itemName));
							}

							_counters.Add(new PerformanceCounterItem(new PerformanceCounter("System", "Processor Queue Length", true), "ProcessorQueueLength"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter("System", "Context Switches/sec", true), "ContextSwitchesSec"));

							_counters.Add(new PerformanceCounterItem(new PerformanceCounter("Paging File", "% Usage", "_Total", true), "PagingFileUsage"));
							_counters.Add(new PerformanceCounterItem(new PerformanceCounter("Memory", "Page Faults/sec", true), "PageFaultsSec"));


							var instancenames = new PerformanceCounterCategory("Network Interface").GetInstanceNames()
								.Where(n => !(n.StartsWith("isatap", StringComparison.OrdinalIgnoreCase) || n.Contains("Loopback") || n.Contains("Pseudo")))
								.ToArray();

							if (instancenames.Any())
							{
								_counters.Add(new PerformanceCounterItem(new PerformanceCounter("Network interface", "Bytes Received/sec", instancenames[0], true), "NetworkReceivedMBytesSec"));
								_counters.Add(new PerformanceCounterItem(new PerformanceCounter("Network interface", "Bytes Sent/sec", instancenames[0], true), "NetworkSentMBytesSec"));
								_counters.Add(new PerformanceCounterItem(new PerformanceCounter("Network interface", "Output Queue Length", instancenames[0], true), "NetworkOutputQueueLength"));
							}
							_initialized = true;
						}
						catch(Exception)
						{
							ResetCounterForUnauthorizedIfRequered();
						}
					}
				}
			}
		}
	}
}
