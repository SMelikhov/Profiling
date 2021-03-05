using System;
using System.Collections.Generic;
using Profiling.Common.DTO;

namespace Profiling.Common.Sampling
{
	public interface IWinFormsLatencyLogger : IDisposable
	{
		void Start();
		void Reset();
		void CollectStacks(int uiThreshold, int uiLatencySnapshotPeriod, bool collectAllStacks);
		void DisposeStacks();
		string Message { get; }
		LatencyIntervalDTO GetLatencyInterval();
		List<Tuple<DateTime, int>> GetLatencyThresholds();
		ProfilingStack[] GetStacks();
		bool TryGetFrozenStack(out ProfilingStack stack);
		void CopyStacks(IStackTracker stackTracker);
		IDisposable Postpone();
		void AddTag(string tag);
	}

	public class WinFormsLatencyLoggerStub : IWinFormsLatencyLogger
	{
		public void Dispose()
		{
		}

		public void Start()
		{
		}

		public void Reset()
		{
		}

		public void CollectStacks(int uiThreshold, int uiLatencySnapshotPeriod, bool collectAllStacks)
		{
		}

		public void DisposeStacks()
		{
		}

		public int Max { get; private set; }
		public int Min { get; private set; }
		public int Avg { get; private set; }
		public int Count { get; private set; }
		public int Elapsed { get; private set; }
		public string Message { get; private set; }

		public LatencyIntervalDTO GetLatencyInterval()
		{
			return null;
		}

		public List<Tuple<DateTime, int>> GetLatencyThresholds()
		{
			return null;
		}

		public ProfilingStack[] GetStacks()
		{
			return null;
		}

		public bool TryGetFrozenStack(out ProfilingStack stack)
		{
			stack = null;
			return false;
		}

		public void CopyStacks(IStackTracker stackTracker)
		{
		}

		public IDisposable Postpone()
		{
			return null;
		}

		public void AddTag(string tag)
		{ 
		}
	}
}
