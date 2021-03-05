using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Profiling.Common.Tracing
{
	partial class Profiling
	{
		internal sealed class ProfilingContextDisposable : IProfilingContextDisposable
		{
			private readonly int _operation;
			private readonly int _entity;
			private readonly string _additionalAggregatedInfo;
			private readonly bool _isUi;
			private readonly int _parentOperationHash;

			private readonly int _correlationHash;
			private readonly ConcurrentQueue<Profiling.QueueItem> _toAdd;
			private readonly IDisposable _disposableOperationScope;
			private readonly Stopwatch _stopwatch;

			private readonly long? _elapsedMilliseconds;

			public ProfilingContextDisposable(
				int operation,
				int entity,
				string additionalAggregatedInfo,
				bool isUi,
				int parentOperationHash,
				int correlationHash,
				ConcurrentQueue<Profiling.QueueItem> toAdd,
				long? elapsedMilliseconds,
				IDisposable disposableOperationScope)
			{
				_operation = operation;
				_entity = entity;
				_additionalAggregatedInfo = additionalAggregatedInfo;
				_isUi = isUi;
				_parentOperationHash = parentOperationHash;
				_correlationHash = correlationHash;
				_toAdd = toAdd;
				_disposableOperationScope = disposableOperationScope;
				_elapsedMilliseconds = elapsedMilliseconds;
				_stopwatch = Stopwatch.StartNew();
			}

			public void Dispose()
			{
				_stopwatch.Stop();
				if (_disposableOperationScope != null)
					_disposableOperationScope.Dispose();

				_toAdd.Enqueue(new Profiling.QueueItem(_operation,
					_parentOperationHash,
					_correlationHash,
					_entity,
					_additionalAggregatedInfo,
					_isUi,
					_elapsedMilliseconds ?? _stopwatch.ElapsedMilliseconds));
			}
		}


	}
}