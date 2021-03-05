using System;

namespace Profiling.Common.Tracing
{
	partial class Profiling
	{
		internal class QueueItem
		{
			private readonly int _operation;
			private readonly int _parentHash;
			private readonly int _correlationHash;
			private readonly int _entity;
			private readonly string _additionalInfo;
			private readonly bool _isUi;
			private readonly long _elapsedMilliseconds;
			private ProfilingDetailKey _detailKey;

			public QueueItem(int operation, int parentHash, int correlationHash, int entity, string additionalInfo, bool isUi, long elapsedMilliseconds)
			{
				_operation = operation;
				_parentHash = parentHash;
				_correlationHash = correlationHash;
				_entity = entity;
				_additionalInfo = additionalInfo;
				_isUi = isUi;
				_elapsedMilliseconds = elapsedMilliseconds;
			}

			public void Init()
			{
				_detailKey = new ProfilingDetailKey(
					_operation,
					_parentHash,
					_entity,
					ParseAdditionalInfo(),
					_isUi,
					_correlationHash
					);
			}

			private string ParseAdditionalInfo()
			{
				return ProfilingSpecificProvider.GetProfilingSpecific().ParseAdditionalInfo(_operation, _additionalInfo);
			}

			public ProfilingDetailKey DetailKey
			{
				get { return _detailKey; }
			}

			public long ElapsedMilliseconds
			{
				get { return _elapsedMilliseconds; }
			}

			public int CorrelationHash
			{
				get { return _correlationHash; }
			}

		}
	}
}