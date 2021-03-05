using System;

namespace Profiling.WindowsForms
{
	public interface ILatencyComponent
	{
		IDisposable Postpone();
	}

	public sealed class LatencyComponentStub : ILatencyComponent
	{
		public IDisposable Postpone()
		{
			return null;
		}
	}
}
