using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Profiling.Common.Sampling
{
	public sealed class ProfilingStackMerger
	{
		private readonly bool _aggregateStacksIntoOne;
		private bool _simplifyStack;
		private readonly ProfilingStackItem[] _items;

		private readonly Dictionary<string, ProfilingStackItem> _roots = new Dictionary<string, ProfilingStackItem>();

		public ProfilingStackMerger(ProfilingStackItem[] roots, bool simplifyStack)
		{
			_aggregateStacksIntoOne = simplifyStack;
			_simplifyStack = simplifyStack;
			_items = DeepClone(roots);

			if (roots.Any())
				Parse();
		}

		public bool SimplifyStack
		{
			get { return _simplifyStack; }
		}

		public ProfilingStackItem[] GetRoots()
		{
			return _roots.Select(c => c.Value).OrderByDescending(c => c.HitCount).ToArray();
		}

		private void Parse()
		{
			var first = _items.First();
			_roots[first.Stack] = first;

			foreach (var item in _items.Skip(1))
			{
				ParseStack(item);
			}

			if (_roots.Any() && _aggregateStacksIntoOne)
			{
				var stack = _roots.First().Value.Stack;
				if (stack.StartsWith("[dbo]")
				    || stack.StartsWith("[reporting]")
				    || stack.StartsWith("[client]"))
				{
					_simplifyStack = false;
					var root = new ProfilingStackItem("SQL");
					foreach (var r in _roots)
					{
						root.AddChild(r.Value);
						root.AddTotalTime(r.Value.TotalTime);
						root.AddHitCount(r.Value.HitCount);
					}

					_roots.Clear();
					_roots[root.Stack] = root;
				}
			}

			foreach (var root in _roots)
			{
				root.Value.SetTotalTime(root.Value.TotalTime);
			}
		}

		private void ParseStack(ProfilingStackItem oldRoot)
		{
			var newItem = AddOrFindRoot(oldRoot);
			if (!ReferenceEquals(newItem, oldRoot))
			{
				newItem.AddTotalTime(oldRoot.TotalTime);
			}
			ParseChild(oldRoot, newItem);
		}

		private void ParseChild(ProfilingStackItem oldItem, ProfilingStackItem newParent)
		{
			foreach (var oldChild in oldItem.Children)
			{
				ProfilingStackItem newItem = newParent.Find(oldChild.Stack);
				if (newItem != null)
				{
					newItem.AddHitCount(oldChild.HitCount);
					newItem.Filtered = newItem.Filtered || oldChild.Filtered;
				}
				else
				{
					newItem = new ProfilingStackItem(oldChild.Stack);
					newItem.AddHitCount(oldChild.HitCount - 1);
					newParent.AddChild(newItem);
				}
				ParseChild(oldChild, newItem);
			}
		}

		private ProfilingStackItem AddOrFindRoot(ProfilingStackItem root)
		{
			ProfilingStackItem item;
			if (_roots.TryGetValue(root.Stack, out item))
			{
				item.AddHitCount(root.HitCount);
				item.Filtered = item.Filtered || root.Filtered;
			}
			else
			{
				item = new ProfilingStackItem(root.Stack);
				_roots.Add(root.Stack, item);
			}
			return item;
		}

		private static T DeepClone<T>(T obj)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}
	}
}
