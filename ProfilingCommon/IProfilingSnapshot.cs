using System;
using Profiling.Common.DTO;
using Profiling.Common.Tracing;

namespace Profiling.Common
{
	internal interface IProfilingSnapshot : IDisposable
	{
		IProfilingContextDisposable ProfileOperation(int operation, int entity, string additionalAggregatedInfo = null, long? elapsedMiliseconds = null);

		ProfilingAggregatedHierarchyItem[] GetAggregatedTree();

		ProfilingAggregatedItemDTO[] GetAggregated();

		void Aggregate();

		ProfilingSettings Settings { get; }

		bool IsProfiling { get; }
	}

	internal class NullableProfilingSnapshot : IProfilingSnapshot
	{
		public void Dispose()
		{

		}

		public IProfilingContextDisposable ProfileOperation(
			int operation, int entity, string additionalAggregatedInfo = null, long? elapsedMiliseconds = null)
		{
			return null;
		}

		public ProfilingAggregatedHierarchyItem[] GetAggregatedTree()
		{
			return new ProfilingAggregatedHierarchyItem[0];
		}

		public ProfilingAggregatedItemDTO[] GetAggregated()
		{
			return new ProfilingAggregatedItemDTO[0];
		}

		public void Aggregate()
		{

		}

		public ProfilingSettings Settings
		{
			get { return new ProfilingSettings(); }
		}

		public bool IsProfiling
		{
			get { return false; }
		}

		public IDisposable ProfileAndCollectStacks()
		{
			return null;
		}
	}
}
