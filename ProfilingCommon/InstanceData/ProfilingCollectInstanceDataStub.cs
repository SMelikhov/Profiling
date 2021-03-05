using System;
using System.Collections.Generic;

namespace Profiling.Common.InstanceData
{
	public sealed class ProfilingCollectInstanceDataStub : IProfilingCollectInstanceData
	{
		public bool IsProfiling => false;

		public List<ProfilingInstanceDataItem> Dequeue()
		{
			return new List<ProfilingInstanceDataItem>();
		}

		public IDisposable ProfileOperation(string operation, long? elapsedMilliseconds, string subOperation, int count,  bool needStartStop, bool highFrequencyOperation, 
			Dictionary<string, string> systremTags, Dictionary<string, object> values)
		{
			return null;
		}

		public void AddProfilingData(IEnumerable<ProfilingInstanceDataItem> items)
		{
		}

		public void SendDataToInfluxDb()
		{
		}

		public void Dispose()
		{
		}
	}
}
