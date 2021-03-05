using System;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	public sealed class MemoryInfoDTO
	{
		[DataMember(Order = 1)]
		public long PrivateBytes64 { get; set; }

		[DataMember(Order = 2)]
		public long VirtualMemorySize64 { get; set; }

		[DataMember(Order = 3)]
		public long WorkingSet64 { get; set; }

		[DataMember(Order = 4)]
		public int Threads { get; set; }



	}
}
