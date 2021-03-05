using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Profiling.Common;
using Profiling.Common.DTO;
using Profiling.Common.Sampling;

namespace Profiling.Client
{
	public class UILatencyManager : ILatencyManager
	{
		private readonly UIOnlyLatencyManager _uiOnlyLatency;
		private readonly LatencyManager _latencyManager;
		private readonly Thread _uiThread;

		public UILatencyManager(SynchronizationContext context, bool isBookMark, ISettingsManagerProfiling settingsManager, Thread uiThread)
		{
			_uiOnlyLatency = new UIOnlyLatencyManager(context, isBookMark, settingsManager, uiThread);
			_latencyManager = new LatencyManager();
			_uiThread = uiThread;
		}

		public void Start(ProfilingSettings setting)
		{
			_uiOnlyLatency.Start(setting);
			_latencyManager.Start(setting);
		}

		public void Stop()
		{
			_uiOnlyLatency.Stop();
			_latencyManager.Stop();			
		}

		public ProfilingLatencyDTO GetAggregatedItem()
		{
			var item = _uiOnlyLatency.GetAggregatedItem();
			if (item == null)
				return null;

			var stacks = _latencyManager.GetAggregatedItem().Stacks;

			if (stacks.Any())
			{
				item.Stacks = item.Stacks.Union(stacks).ToArray();
			}

			return item;
		}

		public ProfilingStack[] GetStacks()
		{
			return _uiOnlyLatency.GetStacks().Union(_latencyManager.GetStacks()).ToArray();
		}

		public LatencyIntervalDTO GetLatencyInterval()
		{
			return _uiOnlyLatency.GetLatencyInterval();
		}

		public List<Tuple<DateTime, int>> GetLatencyThresholds()
		{
			return _uiOnlyLatency.GetLatencyThresholds();
		}

		public ProfilingLatencyDTO GetFrozenAggregatedItem()
		{
			var uiStacks = _uiOnlyLatency.GetFrozenAggregatedItem();

			var stacks = _latencyManager.GetFrozenAggregatedItem();
			if (uiStacks == null && stacks == null)
				return null;

			return new ProfilingLatencyDTO
			{
				LatencyInfo = uiStacks?.LatencyInfo??stacks?.LatencyInfo,
				ReportSlowness = uiStacks?.ReportSlowness??stacks?.ReportSlowness,
				Stacks = (uiStacks?.Stacks ?? new ProfilingStackDTO[0]).Union(stacks?.Stacks??new ProfilingStackDTO[0]).ToArray()
			};
		}

		public IDisposable RegisterThread()
		{
			var thread = Thread.CurrentThread;
			if(thread == _uiThread)
				return null;

			return _latencyManager.RegisterThread();
		}

		public void AddTag(string tag)
		{
			_latencyManager.AddTag(tag);
			_uiOnlyLatency.AddTag(tag);
		}

		public void CopyStacks(IStackTracker stackTracker)
		{
			_uiOnlyLatency.CopyStacks(stackTracker);
			_latencyManager.CopyStacks(stackTracker);
		}

		public IDisposable Postpone()
		{
			return _uiOnlyLatency.Postpone();
		}

		public void Dispose()
		{
			_uiOnlyLatency.Dispose();
			_latencyManager.Dispose();
		}
	}
}
