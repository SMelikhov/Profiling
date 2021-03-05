using System;

namespace Profiling.Common.Tracing
{
	public sealed class ProfilingThreadScope : IDisposable
	{
		private readonly Action _action;
		private bool _disposed;

		internal ProfilingThreadScope(Action action)
		{
			_action = action;
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_action();
				_disposed = true;
			}
		}
	}
}
