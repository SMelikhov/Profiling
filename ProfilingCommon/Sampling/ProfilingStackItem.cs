using System;
using System.Collections.Generic;
using System.Linq;

namespace Profiling.Common.Sampling
{
	[Serializable]
	public sealed class ProfilingStackItem
	{
		private readonly string _stack;
		private readonly Dictionary<string, ProfilingStackItem> _children = new Dictionary<string, ProfilingStackItem>();
		private long _hitCount = 1;
		private decimal _persent;
		private long _totalTime;

		public ProfilingStackItem(string stack)
		{
			_stack = stack;
		}

		public bool Filtered { get; set; }
		public decimal DifferencePercent { get; set; }
		public long OldTotalTime { get; set; }

		public void AddChild(ProfilingStackItem item)
		{
			if (_stack == item.Stack)
				return;

			_children[item.Stack] = item;
		}

		public void IncleaseHitCount()
		{
			_hitCount += 1;
		}

		public void AddHitCount(long hitCount)
		{
			_hitCount += hitCount;
		}

		public string Stack
		{
			get { return _stack; }
		}

		public long HitCount
		{
			get { return _hitCount; }
		}

		public ProfilingStackItem Find(string line)
		{
			ProfilingStackItem item;
			_children.TryGetValue(line, out item);
			return item;
		}

		public List<ProfilingStackItem> Children
		{
			get { return _children.Select(c => c.Value).OrderByDescending(c => c.HitCount).ToList(); }
		}

		public long TotalTime
		{
			get { return _totalTime; }
		}

		public decimal Persent
		{
			get { return _persent; }
		}

		public ProfilingStackItemDTO ToDto()
		{
			var result = new ProfilingStackItemDTO { Stack = Stack, Hints = HitCount, Children = Children.Select(c => c.ToDto()).ToArray() };
			return result;
		}

		public static ProfilingStackItem CreateInstance(ProfilingStackItemDTO dto, long elapsed)
		{
			var result = new ProfilingStackItem(dto.Stack) { _hitCount = dto.Hints };
			if (dto.Children != null)
			{
				foreach (var child in dto.Children)
				{
					result.AddChild(CreateInstance(child, elapsed));
				}
			}
			result.SetTotalTime(elapsed);
			return result;
		}

		public void CreateNewCallGraph(ProfilingStackItem[] children)
		{
			_children.Clear();
			foreach (var item in children)
			{
				AddChild(item);
			}
		}

		public void AddTotalTime(long elapsedMilliseconds)
		{
			_totalTime += elapsedMilliseconds;
		}

		public void SetTotalTimeAndHitCount(long elapsedMilliseconds, int hitCount, long totalTime)
		{
			_totalTime = elapsedMilliseconds;

			_persent = Math.Round(((decimal)(_totalTime * 100)) / totalTime);
		}

		public void SetTotalTime(long elapsedMilliseconds)
		{
			SetChildTotalTime(_hitCount, elapsedMilliseconds);
		}

		private void SetChildTotalTime(long rootHitCount, long elapsedMilliseconds)
		{
			_persent = Math.Round(((decimal)(_hitCount * 100)) / rootHitCount);
			_totalTime = (long)Math.Round(elapsedMilliseconds * _persent / 100, 2);
			foreach (var child in Children)
			{
				child.SetChildTotalTime(rootHitCount, elapsedMilliseconds);
			}
		}
	}
}
