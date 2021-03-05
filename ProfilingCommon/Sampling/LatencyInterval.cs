using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Profiling.Common.DTO;

namespace Profiling.Common.Sampling
{
	public sealed class LatencyInterval
	{
		private const int LatencyThreshold = 5000;
		private List<Tuple<DateTime, int>> _elapsed = new List<Tuple<DateTime, int>>();

		private readonly LatencyIntervalItem[] _items = new[]
			{
				new LatencyIntervalItem(0, 50),
				new LatencyIntervalItem(50, 100),
				new LatencyIntervalItem(100, 300),
				new LatencyIntervalItem(300, 500),
				new LatencyIntervalItem(500, 700),
				new LatencyIntervalItem(700, 1000),
				new LatencyIntervalItem(1000, 2000),
				new LatencyIntervalItem(2000, 3000),
				new LatencyIntervalItem(3000, 5000),
				new LatencyIntervalItem(5000, 10000),
				new LatencyIntervalItem(10000, 20000),
				new LatencyIntervalItem(20000, 60000),
				new LatencyIntervalItem(60000, -1),
			};

		public void Clear()
		{
			for (int i = 0; i < _items.Length; i++)
			{
				_items[i].Clear();
			}
		}

		public void Increment(int elapsed)
		{
			if(elapsed > LatencyThreshold)
				_elapsed.Add(new Tuple<DateTime, int>(DateTime.UtcNow, elapsed));

			for (int i = 0; i < _items.Length; i++)
			{
				_items[i].Increment(elapsed);
			}
		}

		public List<Tuple<DateTime, int>> GetLatencyThresholds()
		{
			var result = _elapsed;

			if (_elapsed.Any())
				_elapsed = new List<Tuple<DateTime, int>>();

			return result;
		}

		public string GetAboveMessageItem()
		{
			var s = new StringBuilder();
			for (int i = 0; i < _items.Length; i++)
			{
				s.Append(_items[i].GetAboveMessageItem());
			}

			return s.ToString();
		}

		public LatencyIntervalDTO GetDto()
		{
			bool hasNotZero = _items.Any(t => t.Above > 0);

			if (!hasNotZero)
				return null;

			int current = 0;

			return new LatencyIntervalDTO
			{
				Below50 = _items[current++].Above,
				Above50 = _items[current++].Above,
				Above100 = _items[current++].Above,
				Above300 = _items[current++].Above,
				Above500 = _items[current++].Above,
				Above700 = _items[current++].Above,
				Above1000 = _items[current++].Above,
				Above2000 = _items[current++].Above,
				Above3000 = _items[current++].Above,
				Above5000 = _items[current++].Above,
				Above10000 = _items[current++].Above,
				Above20000 = _items[current++].Above,
				Above60000 = _items[current++].Above,
			};

		}
	}
}
