using System.Diagnostics;

namespace Profiling.Common.Sampling
{
	public interface ILogEntry
	{
		long ElapsedMilliseconds { get; }
		void Stop();
	}

	public class LogEntry : ILogEntry
	{
		private readonly Stopwatch _clock;

		public LogEntry()
		{
			_clock = Stopwatch.StartNew();
		}

		public void Stop()
		{
			_clock.Stop();
		}

		public long ElapsedMilliseconds
		{
			get { return _clock.ElapsedMilliseconds; }
		}
	}

	public class LogEntryStab : ILogEntry
	{


		public long ElapsedMilliseconds
		{
			get { return 0; }
		}

		public void Stop()
		{
			
		}
	}
}
