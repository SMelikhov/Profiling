using System;
using System.Collections.Generic;
using System.Threading;
using Profiling.Common.DTO;
using Profiling.Common.Sampling;
using Profiling.Common.Util;

namespace Profiling.Client
{
	public sealed class WinFormsLatencyOnlyLogger : IWinFormsLatencyLogger
	{
		private volatile bool _isDisposed;
		private readonly LatencyInterval _interval = new LatencyInterval();

		private volatile int _max;
		private volatile int _avg;
		private volatile int _min = int.MaxValue;
		private volatile int _count;
		private volatile int _elapsed;

		private readonly SynchronizationContext _context;
		private readonly DelayExecuter _delayExecuter;
		private DateTime _start;

		public WinFormsLatencyOnlyLogger(SynchronizationContext context)
		{
			_context = context;

			var samplingInterval = TimeSpan.FromMilliseconds(10);
			_delayExecuter = new DelayExecuter(samplingInterval, StartProbe);

			_delayExecuter.StartOnce();
			_start = DateTime.UtcNow;
		}

		public void Start()
		{
		}

		public void Reset()
		{
			_max = _avg = _count = _elapsed = 0;
			_interval.Clear();
			_min = int.MaxValue;
			_start = DateTime.UtcNow;
		}

		public void DisposeStacks()
		{
		}

		public void CollectStacks(int uiThreshold, int uiLatencySnapshotPeriod, bool collectAllStacks)
		{
		}

		public void CopyStacks(IStackTracker stackTracker)
		{
		}

		public IDisposable Postpone()
		{
			return null;
		}

		public void Dispose()
		{
			_isDisposed = true;

			DisposeStacks();
			_delayExecuter.Dispose();
		}

		public void AddTag(string tag)
		{

		}

		public string Message
		{
			get
			{
				var end = DateTime.UtcNow;

				return string.Format("Elapsed Time: {0} [Max: {1}ms  |  Avg: {2}ms  |  Session Elapsed Time: {3}] ",
					_elapsed.ToString("N0"),
														 _max.ToString("N0"),
														 _avg.ToString("N0"),
														(end - _start).ToString(@"dd\:hh\:mm\:ss"))
							 + _interval.GetAboveMessageItem();
			}
		}

		public LatencyIntervalDTO GetLatencyInterval()
		{
			return _interval.GetDto();
		}

		public List<Tuple<DateTime, int>> GetLatencyThresholds()
		{
			return _interval.GetLatencyThresholds();
		}

		public ProfilingStack[] GetStacks()
		{
			return new ProfilingStack[0];
		}

		public bool TryGetFrozenStack(out ProfilingStack stack)
		{
			stack = null;
			return false;
		}

		private void StartProbe()
		{
			if (_isDisposed)
				return;

			try
			{
				_context.Post(EndProbe, new LogEntry());
			}
			catch (Exception)
			{

				_delayExecuter.StartOnce();
			}
		}

		private void EndProbe(object obj)
		{
			var entry = (ILogEntry)obj;
			if (_isDisposed)
				return;

			try
			{
				entry.Stop();

				_count++;

				var elapsed = (int)entry.ElapsedMilliseconds;

				if (elapsed > _max)
					_max = elapsed;

				if (elapsed < _min)
					_min = elapsed;

				_elapsed += elapsed;

				_avg = _elapsed / CountNotZero;

				_interval.Increment(elapsed);
			}
			finally
			{
				_delayExecuter.StartOnce();
			}
		}

		private int CountNotZero
		{
			get { return _count == 0 ? 1 : _count; }
		}





	}
}
