using System;
using System.Runtime.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	public sealed class ProfilingSettingsDTO
	{
		[DataMember(Order = 1)]
		public int ProfilingLevel;

		[DataMember(Order = 3)]
		public bool CollectLatency;

		[DataMember(Order = 4)]
		public int LatencyThreshold;

		[DataMember(Order = 5)]
		public int LatencySnapshotPeriod;

		[DataMember(Order = 6)]
		public bool StartCollectingBeforeThreshold;

		[DataMember(Order = 7)]
		public int UIFreezeThreshold;


		[DataMember(Order = 10)]
		public bool NeedSaveBookmarkResult;

		[DataMember(Order = 11)]
		public bool CollectInstanceData;

 		[DataMember(Order = 12)]
		public string IncludeProcesses;
	}


}
