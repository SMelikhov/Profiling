namespace Profiling.Common.Tracing
{
	internal sealed class ProfilingAggregatedItem
	{
		private int _count;
		private long _elapsedMilliseconds;

		public ProfilingAggregatedItem(int count, long elapsedMilliseconds)
			: this(count, elapsedMilliseconds, elapsedMilliseconds, elapsedMilliseconds, elapsedMilliseconds)
		{

		}

		public ProfilingAggregatedItem(int count, long elapsedMilliseconds,
			long min, long max, long avg)
		{
			_count = count;
			_elapsedMilliseconds = elapsedMilliseconds;
			Min = min;
			Max = max;
			Avg = avg;
		}

		public int Count
		{
			get { return _count; }
		}

		public long ElapsedMilliseconds
		{
			get { return _elapsedMilliseconds; }
		}

		public long Min
		{
			get;
			private set;
		}

		public long Max
		{
			get;
			private set;
		}

		public long Avg
		{
			get;
			private set;
		}

		public void Aggregate(ProfilingAggregatedItem item)
		{
			if (item.ElapsedMilliseconds < Min)
				Min = item.ElapsedMilliseconds;

			if (item.ElapsedMilliseconds > Max)
				Max = item.ElapsedMilliseconds;

			_count += item.Count;
			_elapsedMilliseconds = ElapsedMilliseconds + item.ElapsedMilliseconds;
			Avg = _elapsedMilliseconds / _count;
		}
	}
}
