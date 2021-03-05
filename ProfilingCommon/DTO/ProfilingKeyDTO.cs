using System;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	public sealed class ProfilingKeyDTO
	{
		[DataMemberAttribute(Order = 1)]
		public Guid SessionId;

		[DataMemberAttribute(Order = 3)]
		public DateTime Date;
	}
}
