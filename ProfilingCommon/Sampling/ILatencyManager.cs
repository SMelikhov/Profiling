using System;
using System.Collections.Generic;
using Profiling.Common.DTO;

namespace Profiling.Common.Sampling
{

	public interface ILatencyManager : IDisposable
	{
		void Stop();

		void Start(ProfilingSettings setting);

		ProfilingLatencyDTO GetAggregatedItem();

		ProfilingLatencyDTO GetFrozenAggregatedItem();

		IDisposable RegisterThread();

		void AddTag(string tag);

		void CopyStacks(IStackTracker stackTracker);

		IDisposable Postpone();

		LatencyIntervalDTO GetLatencyInterval();

		List<Tuple<DateTime, int>> GetLatencyThresholds();
	}

	public class NullableLatencyManager : ILatencyManager
	{
		public void Stop()
		{
			
		}

		public void Start(ProfilingSettings setting)
		{
			
		}

		public ProfilingLatencyDTO GetAggregatedItem()
		{
			return null;
		}

		public ProfilingLatencyDTO GetFrozenAggregatedItem()
		{
			return null;
		}

		public IDisposable RegisterThread()
		{
			return null;
		}

		public void AddTag(string tag)
		{
			
		}

		public void CopyStacks(IStackTracker stackTracker)
		{
			
		}

		public IDisposable Postpone()
		{
			return null;
		}

		public LatencyIntervalDTO GetLatencyInterval()
		{
			return null;
		}

		public List<Tuple<DateTime, int>> GetLatencyThresholds()
		{
			return null;
		}

		public void Dispose()
		{
			
		}
	}
}
