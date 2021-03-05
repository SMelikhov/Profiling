using System;
using System.Diagnostics;
using Profiling.Common.DTO;

namespace Profiling.Common.Tracing
{
	[DebuggerDisplay("Operation{_operation} Entity{_entity}")]
	internal sealed class ProfilingDetailKey : IEquatable<ProfilingDetailKey>
	{
		private readonly int _operation;
		private readonly int _parentOperationHash;
		private readonly int _entity;
		private readonly string _additionalAggregatedInfo;
		private readonly bool _isUi;
		private readonly int _correlationHash;

		public ProfilingDetailKey(int operation, int parentOperationHash, int entity, string additionalAggregatedInfo, bool isUi, int correlationHash)
		{
			_operation = operation;
			_parentOperationHash = parentOperationHash;
			_entity = entity;
			_additionalAggregatedInfo = additionalAggregatedInfo;
			_isUi = isUi;
			_correlationHash = correlationHash;
		}


		public int Operation
		{
			get { return _operation; }
		}

		public int ParentOperationHash
		{
			get { return _parentOperationHash; }
		}

		public int Entity
		{
			get { return _entity; }
		}

		public string AdditionalAggregatedInfo
		{
			get { return _additionalAggregatedInfo; }
		}

		public bool IsUi
		{
			get { return _isUi; }
		}

		public int CorrelationHash
		{
			get { return _correlationHash; }
		}

		public bool Equals(ProfilingDetailKey other)
		{
			return (_operation == other._operation)
				&& (_parentOperationHash == other._parentOperationHash)
				&& (_entity == other._entity)
				&& (_additionalAggregatedInfo == other._additionalAggregatedInfo)
				&& (_isUi == other._isUi)
				&& (_correlationHash == other._correlationHash);
		}

		public override bool Equals(object other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (other.GetType() != typeof(ProfilingDetailKey))
			{
				return false;
			}

			return Equals((ProfilingDetailKey)other);
		}

		public override int GetHashCode()
		{
			return _correlationHash;
		}

		public ProfilingDetailKeyDTO ToDTO()
		{
			return new ProfilingDetailKeyDTO
			{
				AdditionalAggregatedInfo = AdditionalAggregatedInfo,
				Entity = Entity,
				CorrelationHash = CorrelationHash,
				IsUi = IsUi,
				Operation = Operation,
				ParentOperation = ParentOperationHash
			};
		}
	}
}
