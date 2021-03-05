using System;

namespace Profiling.Common.Util
{
	internal sealed class DisposeAction : IDisposable
	{
		private Action _action;

		public DisposeAction(Action action)
		{
			if(action == null)
				throw new ArgumentNullException("action");

			_action = action;
		}

		public void Dispose()
		{
			_action();
		}
	}
}
