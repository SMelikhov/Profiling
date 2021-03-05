using System;
using Profiling.Common.DTO;
using Profiling.Common.Sampling;

namespace Profiling.Common.Tracing
{
	internal sealed class ProfilingAggregatedHierarchyRootItem
	{
		private readonly string _instanceName;
		private readonly Guid _sessionId;
		private readonly DateTime _createdDate;
		private readonly DateTime _changedDate;
		private readonly bool _isSavedBookMark;
		private readonly ProfilingAggregatedHierarchyItem[] _children;
		private readonly ProfilingKey _key;

		public ProfilingAggregatedHierarchyRootItem()
		{

		}

		public ProfilingAggregatedHierarchyRootItem(string instanceName, Guid sessionId, DateTime createdDate, DateTime changedDate, bool isSavedBookMark, ProfilingAggregatedHierarchyItem[] children)
		{
			_instanceName = instanceName;
			_sessionId = sessionId;
			_createdDate = createdDate;
			_changedDate = changedDate;
			_isSavedBookMark = isSavedBookMark;
			_children = children;
			_key = new ProfilingKey(sessionId, createdDate);
		}

		public string InstanceName
		{
			get { return _instanceName; }
		}

		public Guid SessionId
		{
			get { return _sessionId; }
		}

		public DateTime CreatedDate
		{
			get { return _createdDate; }
		}

		public DateTime ChangedDate
		{
			get { return _changedDate; }
		}

		public ProfilingAggregatedHierarchyItem[] Children
		{
			get { return _children; }
		}

		public ProfilingBookmarkMemoryDTO MemoryInfo { get; set; }

		public ProfilingLatency ProfilingLatency
		{
			get;
			set;
		}

		public bool HasLatency { get; set; }

		public ProfilingKey Key
		{
			get { return _key; }
		}

		public bool IsSavedBookMark
		{
			get { return _isSavedBookMark; }
		}
	}
}
