using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Profiling.Common.Tracing
{
	//TODO
	internal interface ITrafficStatistics
	{
		void AddProfile(string methodName, int count);
		IEnumerable<KeyValuePair<string, KeyValuePair<int, int>>> GetProfile();
	}

	internal sealed class TrafficStatisticsManager : ITrafficStatistics
	{
		public readonly static TrafficStatisticsManager Instance = new TrafficStatisticsManager();

		private volatile ITrafficStatistics _trafficStatistics = new TrafficStatisticsStub();

		public void Start()
		{
			_trafficStatistics = new TrafficStatistics();
		}

		public void Stop()
		{
			_trafficStatistics = new TrafficStatisticsStub();
		}

		public void AddProfile(string methodName, int count)
		{
			_trafficStatistics.AddProfile(methodName, count);
		}

		public IEnumerable<KeyValuePair<string, KeyValuePair<int, int>>> GetProfile()
		{
			return _trafficStatistics.GetProfile();
		}
	}

	internal sealed class TrafficStatistics : ITrafficStatistics
	{
		private readonly ConcurrentDictionary<string, KeyValuePair<int, int>> _methodBytes = new ConcurrentDictionary<string, KeyValuePair<int, int>>();

		public void AddProfile(string methodName, int count)
		{
			if (methodName == null || count == 0)
				return;

			KeyValuePair<int, int> value;
			if (_methodBytes.TryGetValue(methodName, out value))
			{
				value = new KeyValuePair<int, int>(value.Key + count, value.Value + 1);
			}
			else
			{
				value = new KeyValuePair<int, int>(count, 1);
			}
			_methodBytes[methodName] = value;
		}

		public IEnumerable<KeyValuePair<string, KeyValuePair<int, int>>> GetProfile()
		{
			return _methodBytes.ToArray();
		}
	}

	internal sealed class TrafficStatisticsStub : ITrafficStatistics
	{
		public void AddProfile(string methodName, int count)
		{

		}

		public IEnumerable<KeyValuePair<string, KeyValuePair<int, int>>> GetProfile()
		{
			return Enumerable.Empty<KeyValuePair<string, KeyValuePair<int, int>>>();
		}
	}
}
