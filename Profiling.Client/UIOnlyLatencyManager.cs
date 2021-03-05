using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Profiling.Common;
using Profiling.Common.DTO;
using Profiling.Common.Sampling;

namespace Profiling.Client
{
	public class UIOnlyLatencyManager : ILatencyManager
	{
		private readonly SynchronizationContext _context;
		private readonly bool _isBookMark;
		private IWinFormsLatencyLogger _logger;
		private readonly ISettingsManagerProfiling _settingsManager;
		private readonly Thread _uiThread;

		public UIOnlyLatencyManager(SynchronizationContext context, bool isBookMark, ISettingsManagerProfiling settingsManager, Thread uiThread)
		{
			_context = context;
			_isBookMark = isBookMark;
			_settingsManager = settingsManager;
			_uiThread = uiThread;
			_logger = new WinFormsLatencyOnlyLogger(context);
		}

		public void Start(ProfilingSettings setting)
		{
			if (_context != null)
			{
				DisposeLatency();
				_logger = new WinFormsLatencyLogger(_context, setting, _isBookMark, _uiThread, _settingsManager);
				_logger.Start();
			}
		}

		public void Stop()
		{
			DisposeLatency();
			_logger = new WinFormsLatencyOnlyLogger(_context);
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
					AdditionalInfo = c.AdditionalInfo,
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
			return _logger?.GetLatencyInterval();
		}

		public List<Tuple<DateTime, int>> GetLatencyThresholds()
		{
			return _logger?.GetLatencyThresholds();
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
					AdditionalInfo = stack.AdditionalInfo,
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

		public void AddTag(string tag)
		{
			_logger?.AddTag(tag);
		}

		public void CopyStacks(IStackTracker stackTracker)
		{
			_logger?.CopyStacks(stackTracker);
		}

		public IDisposable Postpone()
		{
			return _logger?.Postpone();
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
