using System.Threading;
using Profiling.Common.DTO;

namespace Profiling.Common.Tracing
{
	internal class ProfilingSnapshot : IProfilingSnapshot
	{
		private readonly Profiling _profiling;
		private readonly ProfilingLevel _profilingLevel;
		private readonly ProfilingSettings _settings;
		private readonly Thread _uiThread;

		public ProfilingSnapshot(bool processingQueue, ProfilingSettings profilingSettings, Thread uiThread)
		{
			_settings = profilingSettings;
			_uiThread = uiThread;
			_profilingLevel = profilingSettings.ProfilingLevel;
			_profiling = new Profiling(processingQueue);
		}

		public ProfilingSettings Settings
		{
			get { return _settings; }
		}

		public bool IsProfiling
		{
			get { return true; }
		}

		public IProfilingContextDisposable ProfileOperation(
			int operation,
			int entity,
			string additionalAggregatedInfo = null,
			long? elapsedMiliseconds = null)
		{
			if (_profilingLevel == ProfilingLevel.None)
				return null;

			var currentLevel = ProfilingThreadContext.Current.ProfilingLevel;

			var isUi = false;
			if (_profilingLevel == ProfilingLevel.Profiling)
			{
				isUi = _uiThread == Thread.CurrentThread;
			}

			var parentOperationHash = ProfilingThreadContext.Current.ParentOperationHash;
			var currentOperationHash = ProfilingHelper.GetParentHash(operation, entity, additionalAggregatedInfo, isUi, currentLevel);

			var disposableOperationScope = ProfilingThreadContext.Current.CreateProfilingOperationScope(currentOperationHash);

			return _profiling.ProfileOperation(
				operation,
				parentOperationHash,
				entity,
				currentOperationHash,
				additionalAggregatedInfo, isUi, elapsedMiliseconds, disposableOperationScope);
		}

		public ProfilingAggregatedHierarchyItem[] GetAggregatedTree()
		{
			return _profiling.GetAggregatedTree();
		}

		public ProfilingAggregatedItemDTO[] GetAggregated()
		{
			return _profiling.GetAggregated();
		}

		//for UT
		public void Aggregate()
		{
			_profiling.Aggregate();
		}

		public void Dispose()
		{
			_profiling.Dispose();
		}
	}

	internal class ProfilingStackOnlySnapshot : IProfilingSnapshot
	{
		private readonly ProfilingSettings _settings;

		public ProfilingStackOnlySnapshot(ProfilingSettings profilingSettings)
		{
			_settings = profilingSettings;
		}

		public ProfilingSettings Settings
		{
			get { return _settings; }
		}

		public bool IsProfiling
		{
			get { return true; }
		}

		public IProfilingContextDisposable ProfileOperation(
			int operation,
			int entity,
			string additionalAggregatedInfo = null,
			long? elapsedMiliseconds = null)
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

		public void Dispose()
		{

		}
	}
}
