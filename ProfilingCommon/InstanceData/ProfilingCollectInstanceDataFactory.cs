using System;
using System.Collections.Generic;

namespace Profiling.Common.InstanceData
{
	public static class ProfilingCollectInstanceDataFactory
	{
		public static IProfilingCollectInstanceData CreateInstance(bool collectInstanceData, string userName, string url, bool isClientUi, Action<List<ProfilingInstanceDataItem>> sendAction)
		{
			var canSend = !string.IsNullOrEmpty(url);
			if (collectInstanceData && canSend)
			{
				return new ProfilingCollectInstanceData(userName, url, isClientUi, sendAction);
			}

			return new ProfilingCollectInstanceDataStub();
		}
	}
}
