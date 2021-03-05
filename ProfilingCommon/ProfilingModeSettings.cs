using System;

namespace Profiling.Common
{
	public class ProfilingModeSettings : IEquatable<ProfilingModeSettings>
	{
		public const ProfilingModeLevel ProfilingModeLevelDefault = ProfilingModeLevel.None;

		public ProfilingModeLevel ProfilingModeLevel
		{
			get;
			set;
		}

		public ProfilingSettings CustomSettings
		{
			get;
			set;
		}

		public bool Equals(ProfilingModeSettings other)
		{
			return (ProfilingModeLevel == other.ProfilingModeLevel && Equals(CustomSettings, other.CustomSettings));
		}

		public override bool Equals(object other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (other.GetType() != typeof(ProfilingModeSettings))
			{
				return false;
			}

			return Equals((ProfilingModeSettings)other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = (int)ProfilingModeLevel;
				result = (result * 397) ^ CustomSettings.GetHashCode();
				return result;
			}
		}



	}
}
