using System;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	public sealed class ProfilingAggregatedDTO
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
		public int ProfilingId;

		[DataMemberAttribute(Order = 6)]
		public string Latency;

		[DataMemberAttribute(Order = 7)]
		public bool HasLatency;

		[DataMemberAttribute(Order = 8)]
		public string FrozenLatencyData;

		[DataMemberAttribute(Order = 9)]
		public bool IsSavedBookmark;

		[DataMemberAttribute(Order = 10)]
		public DateTime ChangedDate;

		[DataMemberAttribute(Order = 11)]
		public LatencyIntervalDTO LatencyInterval;

		[DataMemberAttribute(Order = 12)]
		public long TotalLatency;

		[DataMemberAttribute(Order = 13)]
		public int StackCount;
	}
}
