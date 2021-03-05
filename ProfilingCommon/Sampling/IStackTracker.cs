using System;
using System.Collections.Generic;
using System.Threading;

namespace Profiling.Common.Sampling
{
	public interface IStackTracker : IDisposable
	{
		ILogEntry CreateLogEntry(Thread thread);
		bool RemoveLogEntry(Thread thread, IEnumerable<string> tags = null);
		ProfilingStack[] GetStacks();
		bool IsFake { get; }
		IDisposable Postpone();
		IDisposable RegisterThread(Thread thread);
		void AddTag(Thread thread,string tag);
		void Parse();
		void CopyStacks(IStackTracker stackTracker);
		bool TryGetFrozenLogEntry(Thread thread, out ProfilingStack stack);
		List<Thread> GetRegisteredThreads();
	}

	public sealed class StackTrackerStub : IStackTracker
	{
		public void Dispose()
		{

		}

		public ILogEntry CreateLogEntry(Thread thread)
		{
			return new LogEntryStab();
		}

		public bool RemoveLogEntry(Thread thread, IEnumerable<string> tags = null)
		{
			return true;
		}

		public ProfilingStack[] GetStacks()
		{
			return new ProfilingStack[0];
		}

		public bool IsFake
		{
			get { return true; }
		}

		public IDisposable Postpone()
		{
			return null;
		}

		public IDisposable RegisterThread(Thread thread)
		{
			return null;
		}

		public void AddTag(Thread thread, string tag)
		{

		}

		public void Parse()
		{
			
		}

		public void CopyStacks(IStackTracker stackTracker)
		{
			
		}

		public bool TryGetFrozenLogEntry(Thread thread, out ProfilingStack stack)
		{
			stack = null;
			return false;
		}

		public List<Thread> GetRegisteredThreads()
		{
			return null;
		}
	}
}