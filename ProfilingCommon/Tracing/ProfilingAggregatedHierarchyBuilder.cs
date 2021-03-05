using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Profiling.Common.DTO;
using Profiling.Common.Sampling;

namespace Profiling.Common.Tracing
{
	internal sealed class ProfilingAggregatedHierarchyBuilder
	{
		private const int MaxNestedLevel = 20;
		private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);
		private static IProfilingSpecific Logger = ProfilingSpecificProvider.GetProfilingSpecific();

		public ProfilingAggregatedHierarchyItem[] Build(Dictionary<ProfilingDetailKey, ProfilingAggregatedItem> items)
		{
			var result = new List<ProfilingAggregatedHierarchyItem>();

			var aggItems = items.Select(kvp =>
			{
				return new ProfilingAggregatedHierarchyItem(kvp.Value,
																										kvp.Key.Operation,
																										kvp.Key.ParentOperationHash,
																										kvp.Key.CorrelationHash,
																										kvp.Key.Entity,
																										kvp.Key.AdditionalAggregatedInfo,
																										kvp.Key.IsUi);
			}).ToList();

			FillChildren(aggItems, result);

			return result.ToArray();
		}

		public ProfilingAggregatedHierarchyRootItem[] Build(ProfilingAggregatedDTO[] items)
		{
			return items.Select(item => new ProfilingAggregatedHierarchyRootItem(item.InstanceName, item.SessionId, item.CreateDate, item.ChangedDate, item.IsSavedBookmark, Build(item.Items))
			{
				ProfilingLatency = new ProfilingLatency(item.Latency, item.FrozenLatencyData),
				HasLatency = item.HasLatency
			}).ToArray();
		}

		public ProfilingAggregatedHierarchyRootItem[] Build(ProfilingBookmarkAggregatedDTO[] items)
		{
			return items.Select(item => new ProfilingAggregatedHierarchyRootItem(item.InstanceName, item.SessionId, item.CreateDate, DateTime.MinValue, false, Build(item.Items))
			{
				MemoryInfo = item.MemoryInfo,
				ProfilingLatency = new ProfilingLatency(item.Latency, item.FrozenLatencyData),
				HasLatency = item.HasLatency
			}).ToArray();
		}

		private ProfilingAggregatedHierarchyItem[] Build(IEnumerable<ProfilingAggregatedItemDTO> items)
		{
			var result = new List<ProfilingAggregatedHierarchyItem>();
			if (items == null)
				return new ProfilingAggregatedHierarchyItem[0];

			var aggItems = items.Select(c =>
			{
				var key = new ProfilingDetailKey(c.Operation, c.ParentOperation,
																				 c.Entity, c.AdditionalAggregatedInfo, c.IsUI, c.CorrelationHash);

				return new ProfilingAggregatedHierarchyItem(
					new ProfilingAggregatedItem(c.Count, c.ElapsedMilliseconds, c.Min, c.Max, c.Avg),
					c.Operation, c.ParentOperation,
					c.CorrelationHash, c.Entity, c.AdditionalAggregatedInfo, c.IsUI);
			}
			).ToList();

			FillChildren(aggItems, result);

			return result.ToArray();
		}

		private void FillChildren(List<ProfilingAggregatedHierarchyItem> aggItems, List<ProfilingAggregatedHierarchyItem> result)
		{
			var task = Task.Factory.StartNew(() =>
			{
				try
				{
					var roots = aggItems.Where(c => c.ParentOperationHash == 0).ToArray();
					var operations = aggItems.Where(c => c.ParentOperationHash != 0).ToArray();
					foreach (var root in roots)
					{
						AddChildren(root, operations, 0);
						result.Add(root);
					}
				}
				catch (Exception ex)
				{
					Logger.Error("FillChildren", ex);
				}
			});

			Task.WaitAny(new Task[] { task }, Timeout);
		}

		private static void AddChildren(ProfilingAggregatedHierarchyItem current, IEnumerable<ProfilingAggregatedHierarchyItem> operations, int currentLevel)
		{
			var parentHash = current.CurrentHash;
			var parentList = operations.Where(c => c.ParentOperationHash == parentHash).ToArray();

			foreach (var item in parentList)
			{
				current.AddChild(item);

				if (currentLevel < MaxNestedLevel)
				{
					AddChildren(item, operations, currentLevel + 1);
				}
			}
		}
	}
}
