using System;
using System.Collections.Generic;

namespace Profiling.Common.InstanceData
{
	public interface IProfilingCollectInstanceData : IDisposable
	{
		List<ProfilingInstanceDataItem> Dequeue();

		IDisposable ProfileOperation(string operation, long? elapsedMilliseconds = null, string subOperation = null, int count = 1, bool needStartStop = false, 
			bool highFrequencyOperation = false, Dictionary<string, string> systemTags = null, Dictionary<string, object> values = null);

		bool IsProfiling { get; }
		void AddProfilingData(IEnumerable<ProfilingInstanceDataItem> items);
		void SendDataToInfluxDb();
	}
}
