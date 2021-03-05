using System;
using System.Linq;
using Profiling.Common.DTO;

namespace Profiling.Common.Sampling
{
	public enum StackType
	{
		Normal = 0,
		Frozen = 1,
		AllStacks = 2,
		Compare = 3,
	}

	public sealed class ProfilingStack
	{
		private readonly long _elapsedMilliseconds;
		private readonly ProfilingStackItem[] _stacks;
		private readonly DateTime _createDate;
		private readonly StackType _stackType;
		private readonly string _additionalInfo;

		public ProfilingStack(long elapsedMilliseconds, DateTime createDate, ProfilingStackItem[] stacks, StackType stackType, string additionalInfo = null)
		{
			_elapsedMilliseconds = elapsedMilliseconds;
			_createDate = createDate;
			_stacks = stacks;
			_stackType = stackType;
			_additionalInfo = additionalInfo;
		}

		public ProfilingStack(ProfilingStackDTO dto)
			: this(dto.ElapsedMilliseconds, dto.CreateDate, dto.Stacks.Select(c => ProfilingStackItem.CreateInstance(c, dto.ElapsedMilliseconds)).ToArray(), (StackType)dto.StackType, dto.AdditionalInfo)
		{

		}

		public long ElapsedMilliseconds
		{
			get { return _elapsedMilliseconds; }
		}

		public ProfilingStackItem[] Stacks
		{
			get { return _stacks; }
		}

		public DateTime CreateDate
		{
			get { return _createDate; }
		}

		public StackType StackType
		{
			get { return _stackType; }
		}

		public string AdditionalInfo
		{
			get { return _additionalInfo; }
		}

		public ProfilingStackDTO ToDto()
		{
			return new ProfilingStackDTO
			{
				ElapsedMilliseconds = ElapsedMilliseconds,
				AdditionalInfo = _additionalInfo,
				CreateDate = CreateDate,
				StackType = (int)StackType,
				Stacks = Stacks.Select(c => c.ToDto()).ToArray()
			};

		}
	}
}
