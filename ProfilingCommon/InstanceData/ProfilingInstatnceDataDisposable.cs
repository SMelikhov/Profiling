using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace Profiling.Common.InstanceData
{
	internal sealed class ProfilingInstanceDataDisposable : IDisposable
	{
		private readonly string _operation;
		private readonly string _subOperation;
		private readonly ConcurrentQueue<QueueInstanceDataItem> _toAdd;
		private readonly long? _elapsedMilliseconds;
		private readonly bool _needStartStop;
		private readonly bool _highFrequencyOperation;
		private readonly Dictionary<string, string> _systemTags;
		private readonly Dictionary<string, object> _values;
		private readonly int _count;
		private readonly DateTime _startTime;
		private readonly Stopwatch _stopwatch;

		public ProfilingInstanceDataDisposable(
			string operation,
			string subOperation,
			ConcurrentQueue<QueueInstanceDataItem> toAdd,
			long? elapsedMilliseconds,
			int count,
			bool needStartStop,
			bool highFrequencyOperation,
			Dictionary<string, string> systemTags,
			Dictionary<string, object> values
			)
		{
			_operation = operation;
			_subOperation = subOperation;
			_toAdd = toAdd;
			_elapsedMilliseconds = elapsedMilliseconds;
			_needStartStop = needStartStop;
			_highFrequencyOperation = highFrequencyOperation;
			_systemTags = systemTags;
			_values = values;
			_count = count;
			_stopwatch = Stopwatch.StartNew();

			if (_needStartStop)
			{
				_startTime = DateTime.UtcNow;
				//usually it is long operation and we can show we started
				_toAdd.Enqueue(new QueueInstanceDataItem(_startTime, _operation + "_Start", _subOperation, _elapsedMilliseconds ?? 1000, _count, _highFrequencyOperation, _systemTags, _values));
			}
		}

		public void Dispose()
		{
			_stopwatch.Stop();

			_toAdd.Enqueue(new QueueInstanceDataItem(DateTime.UtcNow, _operation, _subOperation, _elapsedMilliseconds ?? _stopwatch.ElapsedMilliseconds, _count, _highFrequencyOperation, _systemTags, _values));

			if (_needStartStop)
				_toAdd.Enqueue(new QueueInstanceDataItem(_startTime, _operation + "_Start", _subOperation, _elapsedMilliseconds ?? _stopwatch.ElapsedMilliseconds, _count, _highFrequencyOperation, _systemTags, _values));
		}
	}
}
