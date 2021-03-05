using System.ComponentModel;

namespace Profiling.Common
{
	public enum ProfilingLevel
	{
		None = 0,
		Profiling = 3,
		ProfilingStacksOnly = 4,
	}

	public enum ProfilingModeLevel
	{
		None = 0,
		[Description("Detect Freezing")]
		DetectFreezing = 1,
		[Description("Detect Slowness")]
		DetectSlowness = 2,
		Custom = 3,

	}
}
