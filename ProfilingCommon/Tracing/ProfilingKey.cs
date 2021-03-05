using System;
using Profiling.Common.DTO;

namespace Profiling.Common.Tracing
{
	public sealed class ProfilingKey : IEquatable<ProfilingKey>
	{
		private readonly Guid _sessionId;
		private readonly DateTime _date;

		private readonly int _hash;

		public ProfilingKey(Guid sessionId, DateTime date)
		{
			_sessionId = sessionId;
			_date = date;
			_hash = GetHash(sessionId, date);
		}

		public Guid SessionId
		{
			get { return _sessionId; }
		}

		public DateTime Date
		{
			get { return _date; }
		}

		private static int GetHash(Guid sessionId, DateTime date)
		{
			unchecked
			{
				int result = sessionId.GetHashCode();
				result = (result * 397) ^ date.Day.GetHashCode();
				return result;
			}
		}

		public bool Equals(ProfilingKey other)
		{
			return (_sessionId == other._sessionId)
				&& (_date.Date == other._date.Date);
		}

		public override bool Equals(object other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (other.GetType() != typeof(ProfilingKey))
			{
				return false;
			}

			return Equals((ProfilingKey)other);
		}

		public override int GetHashCode()
		{
			return _hash;
		}

		public ProfilingKeyDTO ToDTO()
		{
			return new ProfilingKeyDTO
			{
				Date = Date,
				SessionId = SessionId
			};
		}
	}
}
