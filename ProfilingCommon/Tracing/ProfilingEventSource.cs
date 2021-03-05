using System.Diagnostics.Tracing;

namespace Profiling.Common.Tracing
{
	[EventSource(Name = ProfilingEventName)]
	public class ProfilingEventSource : EventSource
	{
		public const string ProfilingEventName = "Profiling";

		public static readonly ProfilingEventSource Log = new ProfilingEventSource();

		public void ProfileOperation(long elapsed, int profilingThroughputType, int businessEntity = 0)
		{
			WriteEvent(1, elapsed, profilingThroughputType, businessEntity);
		}

		public void ProfileOperationFailed(int profilingThroughputType, int businessEntity = 0)
		{
			WriteEvent(2, profilingThroughputType, businessEntity);
		}
		
	}
}
