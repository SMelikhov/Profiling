using System.Collections.Generic;
using System.Diagnostics;

namespace Profiling.Common.Tracing
{
	[DebuggerDisplay("Operation {Operation}")]
	internal sealed class ProfilingAggregatedHierarchyItem
	{
		private readonly int _count;
		private readonly long _elapsedMilliseconds;
		private readonly int _operation;
		private readonly int _parentOperationHash;
		private readonly int _correlationHash;
		private readonly int _entity;
		private readonly string _additionalAggregatedInfo;
		private readonly bool _isUi;
		private readonly List<ProfilingAggregatedHierarchyItem> _children = new List<ProfilingAggregatedHierarchyItem>();
		private readonly long _max;
		private readonly long _min;
		private readonly long _avg;
		private readonly ProfilingDetailKey _key;

		public ProfilingAggregatedHierarchyItem()
		{

		}

		public ProfilingAggregatedHierarchyItem(
			ProfilingAggregatedItem item,
			int operation,
			int parentOperationHash,
			int correlationHash,
			int entity,
			string additionalAggregatedInfo,
			bool isUi)
		{
			_operation = operation;
			_parentOperationHash = parentOperationHash;
			_correlationHash = correlationHash;
			_entity = entity;
			_additionalAggregatedInfo = additionalAggregatedInfo;
			_isUi = isUi;

			_count = item.Count;
			_elapsedMilliseconds = item.ElapsedMilliseconds;
			_min = item.Min;
			_max = item.Max;
			_avg = item.Avg;
			_key = new ProfilingDetailKey(operation, parentOperationHash, entity, additionalAggregatedInfo, isUi, correlationHash);
		}

		public int Count
		{
			get { return _count; }
		}

		public long ElapsedMilliseconds
		{
			get { return _elapsedMilliseconds; }
		}

		public long Min
		{
			get { return _min; }
		}

		public long Max
		{
			get { return _max; }
		}

		public long Avg
		{
			get { return _avg; }
		}

		public int Operation
		{
			get { return _operation; }
		}

		public int ParentOperationHash
		{
			get { return _parentOperationHash; }
		}

		public int CurrentHash
		{
			get { return _correlationHash; }
		}

		public int Entity
		{
			get { return _entity; }
		}

		public string AdditionalAggregatedInfo
		{
			get { return _additionalAggregatedInfo; }
		}

		public List<ProfilingAggregatedHierarchyItem> Children
		{
			get { return _children; }
		}

		public bool IsUi
		{
			get { return _isUi; }
		}

		public ProfilingDetailKey Key
		{
			get { return _key; }
		}

		public void AddChildren(IEnumerable<ProfilingAggregatedHierarchyItem> children)
		{
			Children.AddRange(children);
		}

		public void AddChild(ProfilingAggregatedHierarchyItem child)
		{
			//if (!Children.Contains(child))// && Children.All(c => c.CurrentHash != child.CurrentHash) && child.CurrentHash != CurrentHash)
			{
				Children.Add(child);
			}
		}
	}
}
