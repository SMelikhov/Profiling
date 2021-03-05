using System;
using System.Diagnostics;

namespace Profiling.Common.InstanceData
{
	internal class PerformanceCounterItem
	{
		private readonly PerformanceCounter _perfcounter;
		private readonly string _name;

		public PerformanceCounterItem(PerformanceCounter perfcounter, string name)
		{
			_perfcounter = perfcounter;

			_name = name;
		}

		public string Name => _name;

		public float Read()
		{
			var result = _perfcounter?.NextValue() ?? 0;

			if (_name == "ProcessCpu")
				result = result/Environment.ProcessorCount;

			if (_name == "NetworkReceivedMBytesSec" || _name == "NetworkSentMBytesSec" 
			   || _name == "BytesInAllHeaps" || _name == "Gen0HeapSize" || _name == "Gen1HeapSize" || _name == "Gen2HeapSize"
			   || _name == "Gen0Collections" ||  _name == "Gen1Collections" || _name == "Gen2Collections"
				 || _name == "AllocatedBytesSec" || _name == "LargeObjectHeapSize")
				result = result / 1024 / 1024;

			return result;
		}
	}
}
