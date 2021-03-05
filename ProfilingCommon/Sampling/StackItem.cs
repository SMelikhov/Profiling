using System.Collections.Generic;

namespace Profiling.Common.Sampling
{
	public sealed class StackItem
	{
		private readonly long _elapsed;
		private readonly long _failedStacks;
		private readonly float _availableMemory;
		private readonly string[] _stacks;
		private readonly GcItem _gcDif;
		private readonly List<long> _pauses;
		private readonly List<string> _tags;

		public StackItem(long elapsed, long failedStacks, float availableMemory,  string[] stacks, GcItem gcDif = null, List<long> pauses = null, List<string> tags = null)
		{
			_elapsed = elapsed;
			_failedStacks = failedStacks;
			_availableMemory = availableMemory;
			_stacks = stacks;
			_gcDif = gcDif;
			_pauses = pauses;
			_tags = tags;
		}

		public long Elapsed
		{
			get { return _elapsed; }
		}

		public string[] Stacks
		{
			get { return _stacks; }
		}

		public List<long> Pauses
		{
			get { return _pauses; }
		}

		public GcItem GcDif
		{
			get { return _gcDif; }
		}

		public List<string> Tags
		{
			get { return _tags; }
		}

		public long FailedStacks
		{
			get { return _failedStacks; }
		}

		public float AvailableMemory
		{
			get { return _availableMemory; }
		}
	}

}