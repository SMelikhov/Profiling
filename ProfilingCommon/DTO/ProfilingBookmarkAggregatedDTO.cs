using System;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	public sealed class ProfilingBookmarkAggregatedDTO
	{
		[DataMemberAttribute(Order = 1)]
		public ProfilingAggregatedItemDTO[] Items;

		[DataMemberAttribute(Order = 2)]
		public string InstanceName;

		[DataMemberAttribute(Order = 3)]
		public Guid SessionId;

		[DataMemberAttribute(Order = 4)]
		public DateTime CreateDate;

		[DataMemberAttribute(Order = 5)]
		public ProfilingBookmarkMemoryDTO MemoryInfo;

		[DataMemberAttribute(Order = 6)]
		public string Latency;

		[DataMemberAttribute(Order = 7)]
		public bool HasLatency;

		[DataMemberAttribute(Order = 8)]
		public string FrozenLatencyData;

		[DataMemberAttribute(Order = 9)]
		public DateTime ChangedDate;
	}
}
