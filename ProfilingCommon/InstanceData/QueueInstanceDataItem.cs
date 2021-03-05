using System;
using System.Collections.Generic;

namespace Profiling.Common.InstanceData
{
	internal class QueueInstanceDataItem
	{
		public QueueInstanceDataItem(DateTime createTime, string operation, string subOperation, long elapsedMilliseconds, int count, bool highFrequencyOperation, 
			Dictionary<string, string> systemTags, Dictionary<string, object> values)
		{
			SystemTags = systemTags;
			Values = values;
			CreateDate = createTime;
			Operation = operation;
			SubOperation = subOperation;
			ElapsedMilliseconds = elapsedMilliseconds;
			Count = count;
			HighFrequencyOperation = highFrequencyOperation;
		}

		public long ElapsedMilliseconds { get; }

		public string Operation { get; }

		public string SubOperation { get; }

		public DateTime CreateDate { get; }

		public int Count { get; }

		public bool HighFrequencyOperation { get; }

		public Dictionary<string, string> SystemTags { get; }

		public Dictionary<string, object> Values { get; }
	}
}
