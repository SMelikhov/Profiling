using System;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	public sealed class FrozenLatencyDTO
	{
		[DataMemberAttribute(Order = 2)]
		public string InstanceName;

		[DataMemberAttribute(Order = 3)]
		public Guid SessionId;

		[DataMemberAttribute(Order = 4)]
		public DateTime CreateDate;

		[DataMemberAttribute(Order = 8)]
		public string FrozenLatencyData;

		[DataMemberAttribute(Order = 9)]
		public DateTime ChangedDate;
	}
}
