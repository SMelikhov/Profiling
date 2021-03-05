using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	[DebuggerDisplay("Operation {Operation}")]
	public sealed class ProfilingAggregatedItemDTO
	{
		[DataMemberAttribute(Order = 1)]
		public int Count;

		[DataMemberAttribute(Order = 2)]
		public long ElapsedMilliseconds;

		[DataMemberAttribute(Order = 4)]
		public int Operation;

		[DataMemberAttribute(Order = 5)]
		public int ParentOperation;

		[DataMemberAttribute(Order = 6)]
		public int Entity;

		[DataMemberAttribute(Order = 7)]
		public string AdditionalAggregatedInfo;

		[DataMemberAttribute(Order = 8)]
		public long Max;

		[DataMemberAttribute(Order = 9)]
		public long Min;

		[DataMemberAttribute(Order = 10)]
		public long Avg;

		[DataMemberAttribute(Order = 11)]
		public int CorrelationHash;

		[DataMemberAttribute(Order = 14)]
		public bool IsUI;
	}
}
