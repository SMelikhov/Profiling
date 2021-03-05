using System;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	public sealed class LatencyDataDTO
	{
		[DataMemberAttribute(Order = 1)]
		public string Latency;

		[DataMemberAttribute(Order = 2)]
		public string FrozenLatencyData;
	}
}
