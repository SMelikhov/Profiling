using System;
using System.Collections.Generic;
using System.Linq;

namespace Profiling.Common.Sampling
{
	public sealed class ProfilingStackAggregator
	{
		private readonly string[] _stacks;
		private readonly long _elapsedMilliseconds;
		private readonly Dictionary<string, ProfilingStackItem> _roots = new Dictionary<string, ProfilingStackItem>();

		public ProfilingStackAggregator(string[] stacks, long elapsedMilliseconds)
		{
			_stacks = stacks;
			_elapsedMilliseconds = elapsedMilliseconds;
			Parse();
		}

		private void Parse()
		{
			foreach (var stack in _stacks)
			{
				var lines = stack.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

				ParseStack(lines);
			}

			foreach (var root in _roots)
			{
				root.Value.SetTotalTime(_elapsedMilliseconds);
			}
		}

		private void ParseStack(IEnumerable<string> lines)
		{
			ProfilingStackItem parent = null;
			foreach (var rawLine in lines.Reverse().Select(c => c.Trim()))
			{
				string line = rawLine;
				var index = rawLine.IndexOf(":line", StringComparison.Ordinal);
				if (index != -1)
				{
					line = rawLine.Substring(0, index);
				}

				if (parent != null && parent.Stack == line) //dedup
					continue;

				ProfilingStackItem item;

				if (parent == null)
				{
					item = AddOrFindRoot(line);
				}
				else
				{
					item = parent.Find(line);
					if (item != null)
						item.IncleaseHitCount();
					else
					{
						item = new ProfilingStackItem(line);
						parent.AddChild(item);
					}
				}

				parent = item;
			}
		}

		private ProfilingStackItem AddOrFindRoot(string line)
		{
			ProfilingStackItem item;
			if (_roots.TryGetValue(line, out item))
			{
				item.IncleaseHitCount();
			}
			else
			{
				item = new ProfilingStackItem(line);
				_roots.Add(line, item);
			}
			return item;
		}

		public ProfilingStackItem[] GetRoots()
		{
			return _roots.Select(c => c.Value).OrderByDescending(c => c.HitCount).ToArray();
		}
	}
}
