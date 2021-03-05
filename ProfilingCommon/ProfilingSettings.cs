using System;
using Profiling.Common.DTO;

namespace Profiling.Common
{
	public class ProfilingSettings : IEquatable<ProfilingSettings>
	{
		public ProfilingSettings()
			: this(ProfilingLevelDefault, CollectLatencyDefault,
					LatencyThresholdDefault, LatencySnapshotPeriodDefault, StartCollectingBeforeThresholdDefault,
					UIFreezeThresholdDefault, NeedSaveBookmarkResultDefault, 
					CollectInstanceDataDefault, IncludeProcessesDefault)
		{
		}

		public ProfilingSettings(ProfilingSettings profilingSettings)
			: this(profilingSettings.ProfilingLevel, profilingSettings.CollectLatency,
					profilingSettings.LatencyThreshold, profilingSettings.LatencySnapshotPeriod,
					profilingSettings.StartCollectingBeforeThreshold, profilingSettings.UIFreezeThreshold,
					profilingSettings.NeedSaveBookmarkResult,
					profilingSettings.CollectInstanceData, profilingSettings.IncludeProcesses)
		{
		}

		public ProfilingSettings(ProfilingSettingsDTO profilingSettings)
			: this((ProfilingLevel)profilingSettings.ProfilingLevel, profilingSettings.CollectLatency,
					profilingSettings.LatencyThreshold, profilingSettings.LatencySnapshotPeriod,
					profilingSettings.StartCollectingBeforeThreshold, profilingSettings.UIFreezeThreshold,
					profilingSettings.NeedSaveBookmarkResult, 
					profilingSettings.CollectInstanceData, profilingSettings.IncludeProcesses)
		{
		}

		public ProfilingSettings(ProfilingLevel profilingLevel, bool collectLatency, int latencyThreshold, int latencySnapshotPeriod,
			bool startCollectingBeforeThreshold, int uIFreezeThreshold, bool needSaveBookmarkResult, 
			bool collectInstanceData, string includeProcesses)
		{
			ProfilingLevel = profilingLevel;
			CollectLatency = collectLatency;
			LatencyThreshold = latencyThreshold;
			LatencySnapshotPeriod = latencySnapshotPeriod;
			StartCollectingBeforeThreshold = startCollectingBeforeThreshold;
			UIFreezeThreshold = uIFreezeThreshold;
			NeedSaveBookmarkResult = needSaveBookmarkResult;
			CollectInstanceData = collectInstanceData;
			IncludeProcesses = includeProcesses;
		}

		public static ProfilingSettings CreateDetectSlownessMode()
		{
			return new ProfilingSettings
			{
				CollectLatency = true,
				StartCollectingBeforeThreshold = false,
				LatencySnapshotPeriod = 200,
				LatencyThreshold = 2000,
				UIFreezeThreshold = UIFreezeThresholdDefault / 3,
				NeedSaveBookmarkResult = true,
				ProfilingLevel = ProfilingLevel.Profiling
			};
		}

		public static ProfilingSettings CreateDetectFreezingMode()
		{
			return new ProfilingSettings
			{
				CollectLatency = true,
				StartCollectingBeforeThreshold = false,
				LatencySnapshotPeriod = 500,
				LatencyThreshold = Math.Min(UIFreezeThresholdDefault, 60 * 1000),
				UIFreezeThreshold = UIFreezeThresholdDefault,
				NeedSaveBookmarkResult = true,
				ProfilingLevel = ProfilingLevel.ProfilingStacksOnly
			};
		} 

		public static ProfilingSettings CreateNoneMode()
		{
			return new ProfilingSettings();
		}

		public static ProfilingSettings CreateMode(ProfilingModeLevel profilingModeLevel)
		{
			switch (profilingModeLevel)
			{
				case ProfilingModeLevel.DetectSlowness:
					return CreateDetectSlownessMode();
				case ProfilingModeLevel.DetectFreezing:
					return CreateDetectFreezingMode();
				case ProfilingModeLevel.None:
					return CreateNoneMode();
				default:
					return null;
			}
		}

		public static ProfilingSettings CreateInstance(ProfilingModeSettings modeSettings)
		{
			var result = new ProfilingSettings();
			var mode = CreateMode(modeSettings.ProfilingModeLevel);
			if (mode != null)
			{
				result.Copy(mode);
				result.CopyCommon(modeSettings.CustomSettings);
			}
			else
			{
				result.Copy(modeSettings.CustomSettings);
			}
			return result;
		}

		public const ProfilingLevel ProfilingLevelDefault = ProfilingLevel.None;

		public ProfilingLevel ProfilingLevel
		{
			get;
			set;
		}

		public const bool CollectLatencyDefault = false;

		public bool CollectLatency
		{
			get;
			set;
		}

		public const int LatencyThresholdDefault = 5000;

		public int LatencyThreshold
		{
			get;
			set;
		}

		public const int LatencySnapshotPeriodDefault = 100;

		public int LatencySnapshotPeriod
		{
			get;
			set;
		}

		public const bool StartCollectingBeforeThresholdDefault = false;

		public bool StartCollectingBeforeThreshold
		{
			get;
			set;
		}

		public const int UIFreezeThresholdDefault = 45 * 1000;

		public int UIFreezeThreshold
		{
			get;
			set;
		}

		public const bool NeedSaveBookmarkResultDefault = false;

		public bool NeedSaveBookmarkResult
		{
			get;
			set;
		}

		public const bool CollectInstanceDataDefault = true;

		public bool CollectInstanceData
		{
			get;
			set;
		}

		public const string IncludeProcessesDefault = "Client";

		public string IncludeProcesses
		{
			get;
			set;
		}

		public bool Equals(ProfilingSettings other)
		{
			return (ProfilingLevel == other.ProfilingLevel)
				&& (CollectLatency == other.CollectLatency)
				&& (LatencyThreshold == other.LatencyThreshold)
				&& (StartCollectingBeforeThreshold == other.StartCollectingBeforeThreshold)
				&& (UIFreezeThreshold == other.UIFreezeThreshold)
				&& (NeedSaveBookmarkResult == other.NeedSaveBookmarkResult)
				&& (CollectInstanceData == other.CollectInstanceData)
				&& (IncludeProcesses == other.IncludeProcesses)
				&& (LatencySnapshotPeriod == other.LatencySnapshotPeriod);
		}

		public override bool Equals(object other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (other.GetType() != typeof(ProfilingSettings))
			{
				return false;
			}

			return Equals((ProfilingSettings)other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = (int)ProfilingLevel;
				result = (result * 397) ^ (CollectLatency ? 10 : 1);
				result = (result * 397) ^ (NeedSaveBookmarkResult ? 10 : 1);
				result = (result * 397) ^ (CollectInstanceData ? 10 : 1);
				result = (result * 397) ^ (IncludeProcesses??"").GetHashCode();
				result = (result * 397) ^ LatencyThreshold;
				result = (result * 397) ^ LatencySnapshotPeriod;
				result = (result * 397) ^ UIFreezeThreshold;
				result = (result * 397) ^ (StartCollectingBeforeThreshold ? 10 : 1);
				return result;
			}
		}

		public ProfilingSettingsDTO ToDTO()
		{
			return new ProfilingSettingsDTO
			{
				CollectLatency = CollectLatency,
				ProfilingLevel = (int)ProfilingLevel,
				LatencyThreshold = LatencyThreshold,
				LatencySnapshotPeriod = LatencySnapshotPeriod,
				StartCollectingBeforeThreshold = StartCollectingBeforeThreshold,
				UIFreezeThreshold = UIFreezeThreshold,
				NeedSaveBookmarkResult = NeedSaveBookmarkResult,
				CollectInstanceData = CollectInstanceData,
				IncludeProcesses = IncludeProcesses,
			};
		}


		public void Copy(ProfilingSettings settings)
		{
			CollectLatency = settings.CollectLatency;
			ProfilingLevel = settings.ProfilingLevel;
			LatencyThreshold = settings.LatencyThreshold;
			LatencySnapshotPeriod = settings.LatencySnapshotPeriod;
			StartCollectingBeforeThreshold = settings.StartCollectingBeforeThreshold;
			UIFreezeThreshold = settings.UIFreezeThreshold;
			NeedSaveBookmarkResult = settings.NeedSaveBookmarkResult;
			CollectInstanceData = settings.CollectInstanceData;
			IncludeProcesses = settings.IncludeProcesses;
		}

		public void CopyCommon(ProfilingSettings settings)
		{
			CollectInstanceData = settings.CollectInstanceData;
			IncludeProcesses = settings.IncludeProcesses;
		}


		public override string ToString()
		{
			return string.Format(
				"CollectLatency={1}{0}ProfilingLevel={2}{0}LatencyThreshold={3}{0}LatencySnapshotPeriod={4}{0}StartCollectingBeforeThreshold={5}{0}UIFreezeThreshold={6}{0}NeedSaveBookmarkResult={7}{0}CollectInstanceData={8}{0}IncludeProcesses=[{9}]{0}LogExcessiveMemoryAllocationIncidents=[{10}]{0}",
				Environment.NewLine, CollectLatency, ProfilingLevel, LatencyThreshold, LatencySnapshotPeriod, StartCollectingBeforeThreshold,
				UIFreezeThreshold, NeedSaveBookmarkResult, CollectInstanceData, IncludeProcesses);
		}
	}
}
