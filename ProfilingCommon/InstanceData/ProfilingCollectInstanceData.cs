using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Profiling.Common.InstanceData
{
	public sealed class ProfilingCollectInstanceData : IProfilingCollectInstanceData
	{
		private readonly Action<List<ProfilingInstanceDataItem>> _sendAction;
		private readonly List<ProfilingInstanceDataItem> _instanceData = new List<ProfilingInstanceDataItem>();
		private readonly Timer _sendTimer;
		private readonly Timer _environmentTimer;
		private readonly ConcurrentQueue<QueueInstanceDataItem> _toAdd = new ConcurrentQueue<QueueInstanceDataItem>();
		private readonly PerformanceCounters _counters;
		private readonly object _sync = new object();
		private readonly InfluxDbSender _sender;

		public ProfilingCollectInstanceData(string userName, string url, bool isClientUi, Action<List<ProfilingInstanceDataItem>> sendAction)
		{
			_sendAction = sendAction;
			_counters = new PerformanceCounters(isClientUi);
			_sender = new InfluxDbSender(url,  isClientUi ? userName : null);
			_sendTimer = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds)
			{
				Enabled = true
			};
			_sendTimer.Elapsed +=  AddPoints;

			_environmentTimer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds)
			{
				Enabled = true
			};
			_environmentTimer.Elapsed += AddEnvironmentPoints;		
		}

		public bool IsProfiling => true;

		public IDisposable ProfileOperation(string operation, long? elapsedMilliseconds, string subOperation = null, int count = 1, bool needStartStop = false, 
			bool highFrequencyOperation = false, Dictionary<string, string> systemTags = null, Dictionary<string, object> values = null)
		{
			return new ProfilingInstanceDataDisposable(operation, subOperation, _toAdd, elapsedMilliseconds, count, needStartStop, highFrequencyOperation, systemTags, values);
		}

		public void AddProfilingData(IEnumerable<ProfilingInstanceDataItem> items)
		{
			lock (_sync)
			{
				_instanceData.AddRange(items);
			}
		}

		public List<ProfilingInstanceDataItem> Dequeue()
		{
			lock (_sync)
			{
				var result = _instanceData.ToList();
				_instanceData.Clear();
				return result;
			}
		}

		public void SendDataToInfluxDb()
		{
			lock (_sync)
			{
				QueueInstanceDataItem item;
				while (_toAdd.TryDequeue(out item))
				{
					_instanceData.Add(new ProfilingInstanceDataItem
					{
						Operation = item.Operation,
						SubOperation = item.SubOperation,
						ElapsedMilliseconds = item.ElapsedMilliseconds,
						HighFrequencyOperation = item.HighFrequencyOperation,
						CreateDate = item.CreateDate,
						SystemTags = item.SystemTags,
						Values = item.Values,
						Count = item.Count,
					});
				}
			}

			Publish();
		}

		private void Publish()
		{
			try
			{
				var items = Dequeue();
				if (_sendAction != null)
					_sendAction(items);
				else
					_sender.SaveToInfluxDb(items);
			}
			catch (Exception)
			{
				// ignored
			}
		}

		private void AddPoints(object sender, ElapsedEventArgs e)
		{
			SendDataToInfluxDb();
		}

		private void AddEnvironmentPoints(object sender, ElapsedEventArgs e)
		{
			lock (_sync)
			{
				var systemMemory = MemoryProfiling.GetAvailable();
				var processMemory = MemoryProfiling.GetMemoryInfo();
				var perfCounters = _counters.GetStatistics();

				foreach (var perfCounter in perfCounters)
				{
					_instanceData.Add(new ProfilingInstanceDataItem
					{
						Operation = perfCounter.Key,
						ElapsedMilliseconds = perfCounter.Value,
						CreateDate = DateTime.UtcNow,
					});					
				}

				_instanceData.Add(new ProfilingInstanceDataItem
				{
					Operation = "Available",
					ElapsedMilliseconds = systemMemory.Available,
					CreateDate = DateTime.UtcNow,
				});	
						
				_instanceData.Add(new ProfilingInstanceDataItem
				{
					Operation = "Total",
					ElapsedMilliseconds = systemMemory.Total,
					CreateDate = DateTime.UtcNow,
				});		

				_instanceData.Add(new ProfilingInstanceDataItem
				{
					Operation = "Usage",
					ElapsedMilliseconds = systemMemory.Usage,
					CreateDate = DateTime.UtcNow,
				});	

				_instanceData.Add(new ProfilingInstanceDataItem
				{
					Operation = "VirtualMemorySize64",
					ElapsedMilliseconds = processMemory.VirtualMemorySize64,
					CreateDate = DateTime.UtcNow,
				});	

				_instanceData.Add(new ProfilingInstanceDataItem
				{
					Operation = "PrivateBytes64",
					ElapsedMilliseconds = processMemory.PrivateBytes64,
					CreateDate = DateTime.UtcNow,
				});	

				_instanceData.Add(new ProfilingInstanceDataItem
				{
					Operation = "WorkingSet64",
					ElapsedMilliseconds = processMemory.WorkingSet64,
					CreateDate = DateTime.UtcNow,
				});	

				_instanceData.Add(new ProfilingInstanceDataItem
				{
					Operation = "Threads",
					ElapsedMilliseconds = processMemory.Threads,
					CreateDate = DateTime.UtcNow,
				});	
			}
		}

		public void Dispose()
		{
			_sendTimer?.Stop();
			_sendTimer?.Dispose();

			_environmentTimer?.Stop();
			_environmentTimer?.Dispose();
		}
	}
}
