using System;
using System.Threading;

namespace Profiling.Common
{
	internal sealed class Job : IDisposable
	{
		private readonly Action _action;
		private readonly TimeSpan _delayTime;

		private volatile bool _isRunning;
		private long _enabled;
		private long _workQueue;

		public Job(Action action)
			: this(action, TimeSpan.Zero)
		{
		}

		public Job(Action action, TimeSpan delayTime)
		{
			if (action == null)
				throw new ArgumentException("action");

			_action = action;
			_delayTime = delayTime;
		}

		public void Dispose()
		{
			Enabled = false;
		}

		public bool Enabled
		{
			get
			{
				return Interlocked.Read(ref _enabled) == 1;
			}
			set
			{
				var flag = Interlocked.Exchange(ref _enabled, value ? 1 : 0);

				if (flag == 0)
				{
					// == there is no worker thread running
					// so first work request after setting Enabled to true will start worker thread
					Interlocked.Exchange(ref _workQueue, 0);
				}
			}
		}

		public void PerformRecalculation()
		{
			QueueWork();
		}

		private void PerformJob(object dummy)
		{
			// it is still possible for 2 or more threads to enter here
			SpinWait.SpinUntil(() => !_isRunning);

			_isRunning = true;

			while (Enabled)
			{
				if (_delayTime.TotalMilliseconds > 0)
				{
					// Sleep was originally placed _before_ executing action
					// Possible reason: multiple sources signalling to perform job, but we want to do it once.
					// So this Sleep works like aggregate window
					Thread.Sleep(_delayTime);
				}

				var queueSize = GetWorkQueueSize();

				try
				{
					_action();
				}
				catch (Exception)
				{
					//add logging
				}

				if (!HasMoreWork(queueSize))
				{
					break;
				}

				StartOver();
			}

			_isRunning = false;
		}

		private void QueueWork()
		{
			if (!Enabled)
				return;

			var value = Interlocked.Increment(ref _workQueue);
			var startWorker = value == 1;
			if (startWorker)
			{
				ThreadPool.QueueUserWorkItem(PerformJob);
			}
		}

		private long GetWorkQueueSize()
		{
			return Interlocked.Read(ref _workQueue);
		}

		private void StartOver()
		{
			// do not set 0, because it means no thread is doing work!
			Interlocked.Exchange(ref _workQueue, 1);
		}

		private bool HasMoreWork(long previous)
		{
			var workQueue = Interlocked.CompareExchange(ref _workQueue, 0, previous);

			var queueCleared = workQueue == 0;
			var queueChanged = workQueue > previous;
			if (queueCleared || !queueChanged)
				return false;

			return true;
		}
	}
}
