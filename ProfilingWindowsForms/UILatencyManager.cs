using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Profiling.Common;
using Profiling.Common.DTO;
using Profiling.Common.Sampling;

namespace Profiling.WindowsForms
{
	public class UILatencyManager : ILatencyManager
	{
		private readonly Control _parent;
		private readonly bool _isBookMark;
		private IWinFormsLatencyLogger _logger;
		private readonly ISettingsManagerProfiling _settingsManager;
		private readonly Thread _uiThread;

		public UILatencyManager(Control parent, bool isBookMark, ISettingsManagerProfiling settingsManager, Thread uiThread)
		{
			_parent = parent;
			_isBookMark = isBookMark;
			_settingsManager = settingsManager;
			_uiThread = uiThread;
		}

		public void Start(ProfilingSettings setting)
		{
			if (_parent != null)
			{
				DisposeLatency();
				_logger = new WinFormsLatencyLogger(_parent, setting, _isBookMark, _uiThread, _settingsManager);
				_logger.Start();
			}
		}

		public void Stop()
		{
			DisposeLatency();
		}

		public ProfilingLatencyDTO GetAggregatedItem()
		{
			if (_logger == null)
				return null;

			var item = new ProfilingLatencyDTO
			{
				LatencyInfo = _logger.Message,
				Stacks = _logger.GetStacks()
				.Select(c => new ProfilingStackDTO
				{
					ElapsedMilliseconds = c.ElapsedMilliseconds,
					CreateDate = c.CreateDate,
					StackType = (int)c.StackType,
					Stacks = c.Stacks.Select(i => i.ToDto()).ToArray()
				})
				.ToArray()
			};

			return item;
		}

		public ProfilingStack[] GetStacks()
		{
			if (_logger == null)
				return new ProfilingStack[0];

			return _logger.GetStacks();
		}

		public LatencyIntervalDTO GetLatencyInterval()
		{
			if (_logger == null)
				return null;

			return _logger.GetLatencyInterval();
		}

		public ProfilingLatencyDTO GetFrozenAggregatedItem()
		{
			if (_logger == null)
				return null;

			ProfilingStack stack;
			if (!_logger.TryGetFrozenStack(out stack))
				return null;

			var item = new ProfilingLatencyDTO
			{
				LatencyInfo = _logger.Message,
				Stacks = new[]{ new ProfilingStackDTO
				{
					ElapsedMilliseconds = stack.ElapsedMilliseconds,
					CreateDate = stack.CreateDate,
					StackType = (int)stack.StackType,
					Stacks = stack.Stacks.Select(i => i.ToDto()).ToArray()
				}}
			};

			return item;
		}

		public ProfilingLatencyDTO TryGetStacksForAllThreads()
		{
			if (_logger == null)
				return null;

			ProfilingStack stack;
			if (!_logger.TryGetStacksForAllThreads(out stack))
				return null;

			var item = new ProfilingLatencyDTO
			{
				LatencyInfo = _logger.Message,
				Stacks = new[]{ new ProfilingStackDTO
				{
					ElapsedMilliseconds = stack.ElapsedMilliseconds,
					CreateDate = stack.CreateDate,
					StackType = (int)stack.StackType,
					Stacks = stack.Stacks.Select(i => i.ToDto()).ToArray()
				}}
			};

			return item;
		}

		public IDisposable RegisterThread()
		{
			return null;
		}

		public void CopyStacks(IStackTracker stackTracker)
		{
			if (_logger != null)
			{
				_logger.CopyStacks(stackTracker);
			}
		}

		public IDisposable Postpone()
		{
			if (_logger != null)
			{
				return _logger.Postpone();
			}
			return null;
		}

		private void DisposeLatency()
		{
			if (_logger != null)
			{
				_logger.Dispose();
				_logger = null;
			}
		}

		public void Dispose()
		{
			DisposeLatency();
			_settingsManager.Dispose();
		}
	}
}
