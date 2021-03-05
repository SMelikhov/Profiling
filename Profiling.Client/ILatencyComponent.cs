using System;

namespace Profiling.Client
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
