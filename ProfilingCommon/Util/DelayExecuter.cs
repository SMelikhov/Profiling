using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Profiling.Common.Util
{
	public sealed class DelayExecuter : IDisposable
	{
		private readonly Action _action;
		private readonly bool _startStop;
		private readonly Timer _timer;
		private long _enabled;

		public DelayExecuter(TimeSpan interval, Action action, bool startStop = false)
		{
			_action = action;
			_startStop = startStop;
			_timer = new Timer(interval.TotalMilliseconds)
			{
				AutoReset = true,
				Enabled = true
			};

			_timer.Elapsed += (obj, args) =>
			{
				if (_startStop)
					_timer.Stop();

				var flag = Interlocked.Exchange(ref _enabled, 0);
				if (flag != 0)
				{
					_action();
				}
			};
		}

		public void StartOnce()
		{
			Interlocked.Exchange(ref _enabled, 1);
			if (_startStop)
			{
				_timer.Stop();
				_timer.Start();
			}
		}

		public void Dispose()
		{
			_timer.Dispose();
		}
	}
}
