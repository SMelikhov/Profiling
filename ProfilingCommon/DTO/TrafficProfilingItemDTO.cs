using System;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	public sealed class TrafficProfilingItemDTO
	{
		[DataMemberAttribute(Order = 1)]
		public int Count;

		[DataMemberAttribute(Order = 2)]
		public int TrafficBytes;

		[DataMemberAttribute(Order = 3)]
		public string MethodName;
	}
}
