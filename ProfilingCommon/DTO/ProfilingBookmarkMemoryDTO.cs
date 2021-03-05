using System;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	public sealed class ProfilingBookmarkMemoryDTO
	{
		[DataMemberAttribute(Order = 1)]
		public MemoryInfoDTO MemoryInfo;

		[DataMemberAttribute(Order = 2)]
		public DataContextMemoryProfilingItemDTO[] DataContextItems;

		[DataMemberAttribute(Order = 3)]
		public TrafficProfilingItemDTO[] TrafficItems;
	}
}
