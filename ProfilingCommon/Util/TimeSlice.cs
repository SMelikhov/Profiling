using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiling.Common.Util
{
	internal class TimeSlice
	{
		private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
		private readonly long _quantum;

		private TimeSlice(long quantum)
		{
			_quantum = quantum;
		}

		public static TimeSlice Start(TimeSpan quantum)
		{
			return new TimeSlice((long)quantum.TotalMilliseconds);
		}

		public static TimeSlice Start(long quantum)
		{
			return new TimeSlice(quantum);
		}

		public bool HasExpired
		{
			get { return _stopwatch.ElapsedMilliseconds > _quantum; }
		}

		public long ElapsedMilliseconds
		{
			get { return _stopwatch.ElapsedMilliseconds; }
		}

		public void Restart()
		{
			_stopwatch.Restart();
		}
	}
}
