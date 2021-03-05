using System;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	public sealed class ProfilingDetailKeyDTO
	{
		[DataMemberAttribute(Order = 1)]
		public int Operation;

		[DataMemberAttribute(Order = 2)]
		public int ParentOperation;

		[DataMemberAttribute(Order = 3)]
		public int Entity;

		[DataMemberAttribute(Order = 4)]
		public string AdditionalAggregatedInfo;

		[DataMemberAttribute(Order = 5)]
		public bool IsUi;

		[DataMemberAttribute(Order = 6)]
		public int CorrelationHash;
	}
}
