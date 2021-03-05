namespace Profiling.Common.InstanceData
{
	internal sealed class AggregatedInfluxItem
	{
		public AggregatedInfluxItem(int count, double elapsedMilliseconds)
		{
			Count = count;
			ElapsedMilliseconds = elapsedMilliseconds;
		}

		public int Count { get; private set; }

		public double ElapsedMilliseconds { get; private set; }

		public void SetReported()
		{
			Count = -1;
			ElapsedMilliseconds = -1;
		}

		public bool IsReported => Count == -1;
	}
}
