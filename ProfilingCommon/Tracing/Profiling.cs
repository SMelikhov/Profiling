using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Profiling.Common.DTO;

namespace Profiling.Common.Tracing
{
	internal sealed partial class Profiling : IDisposable
	{
		private readonly object _sync = new object();
		private static readonly ProfilingAggregatedItem[] Empty = new ProfilingAggregatedItem[0];
		private const int PollingSleepInterval = 5000;
		private readonly AutoResetEvent _addToPollEvent = new AutoResetEvent(false);
		private readonly EventWaitHandle _stopEvent = new ManualResetEvent(false);
		private readonly EventWaitHandle[] _waitHandles;
		private readonly Dictionary<ProfilingDetailKey, ProfilingAggregatedItem> _items = new Dictionary<ProfilingDetailKey, ProfilingAggregatedItem>();
		private readonly ConcurrentQueue<QueueItem> _toAdd = new ConcurrentQueue<QueueItem>();
		private readonly Task _task;
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();
		private bool _isDisposed = false;

		public Profiling(bool processingQueue)
		{
			_waitHandles = new[] { _stopEvent, _addToPollEvent };
			if (processingQueue)
			{
				_task = Task.Factory.StartNew(PollingWorkThread, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
			}
		}

		//this function should be fast and non blocking
		public IProfilingContextDisposable ProfileOperation(
			int operation,
			int parentOperationHash,
			int entity,
			int correlationHash,
			string additionalAggregatedInfo,
			bool isUi,
			long? elepsedMliseconds,
			IDisposable disposableOperationScope = null)
		{
			return new ProfilingContextDisposable(operation,
				entity,
				additionalAggregatedInfo,
				isUi,
				parentOperationHash,
				correlationHash,
				_toAdd,
				elepsedMliseconds,
				disposableOperationScope);
		}

		//for UT
		internal ProfilingAggregatedItem[] GetAggregated(
			int operation,
			int parentOperationHash,
			int correlationHash,
			int entity,
			string additionalAggregatedInfo = null,
			bool isUi = false)
		{
			var key = new ProfilingDetailKey(operation,
				parentOperationHash, entity, additionalAggregatedInfo, isUi, correlationHash);

			lock (_sync)
			{
				ProfilingAggregatedItem item;
				if (_items.TryGetValue(key, out item))
				{
					return new[] { item };
				}
			}
			return Empty;
		}

		public ProfilingAggregatedItemDTO[] GetAggregated()
		{
			lock (_sync)
			{
				return _items.Select(
					kvp =>
					{
						return new ProfilingAggregatedItemDTO
						{
							AdditionalAggregatedInfo = kvp.Key.AdditionalAggregatedInfo,
							IsUI = kvp.Key.IsUi,
							Count = kvp.Value.Count,
							Max = kvp.Value.Max,
							Min = kvp.Value.Min,
							Avg = kvp.Value.Avg,
							ElapsedMilliseconds = kvp.Value.ElapsedMilliseconds,
							Entity = (int)kvp.Key.Entity,
							Operation = (int)kvp.Key.Operation,
							ParentOperation = kvp.Key.ParentOperationHash,
							CorrelationHash = kvp.Key.CorrelationHash,
						};
					}).ToArray();
			}
		}

		public ProfilingAggregatedHierarchyItem[] GetAggregatedTree()
		{
			var builder = new ProfilingAggregatedHierarchyBuilder();
			lock (_sync)
			{
				return builder.Build(_items);
			}
		}

		//for UT
		public void Aggregate()
		{
			Profiling.QueueItem queueItem;
			while (_toAdd.TryDequeue(out queueItem))
			{
				lock (_sync)
				{
					queueItem.Init();
					var aggregatedItem = new ProfilingAggregatedItem(1, queueItem.ElapsedMilliseconds);

					ProfilingAggregatedItem item;
					if (_items.TryGetValue(queueItem.DetailKey, out item))
					{
						item.Aggregate(aggregatedItem);
					}
					else
					{
						_items.Add(queueItem.DetailKey, aggregatedItem);
					}
				}
			}
		}

		private void PollingWorkThread()
		{
			while (!_cts.IsCancellationRequested)
			{
				int eventIndex = WaitHandle.WaitAny(_waitHandles, PollingSleepInterval, false);

				if (eventIndex != WaitHandle.WaitTimeout)
				{
					if (_waitHandles[eventIndex] == _stopEvent)
						return;
				}

				Aggregate();
			}
		}

		public void Dispose()
		{
			if (_isDisposed)
				return;

			try
			{
				_stopEvent.Set();

				if (!_task.IsCompleted)
					_cts.Cancel();

				_task.Wait(TimeSpan.FromSeconds(10));
				_task.Dispose();
				_cts.Dispose();
			}
			catch
			{
				// ignored
			}

			_isDisposed = true;
		}
	}
}
