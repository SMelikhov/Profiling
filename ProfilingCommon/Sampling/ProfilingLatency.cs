using System.Linq;
using Profiling.Common.DTO;

namespace Profiling.Common.Sampling
{
	public sealed class ProfilingLatency
	{
		private readonly ProfilingReportSlownessDTO[] _reportSlowness;
		private readonly string _latencyInfo;
		private readonly ProfilingStack[] _stacks;

		public ProfilingLatency(string latency, string frozenLatency)
		{
			var dto = ProfilingHelper.DeserializeProfilingLatency(latency);
			var frozenDto = ProfilingHelper.DeserializeProfilingLatency(frozenLatency);

			if (dto == null && frozenDto == null)
				return;

			if (dto != null)
				_latencyInfo = dto.LatencyInfo;
			else if (frozenDto != null)
				_latencyInfo = frozenDto.LatencyInfo;

			_stacks =
				(dto == null || dto.Stacks == null ? new ProfilingStackDTO[0] : dto.Stacks)
				.Union(frozenDto == null || frozenDto.Stacks == null ? new ProfilingStackDTO[0] : frozenDto.Stacks)
				.Select(c => new ProfilingStack(c.ElapsedMilliseconds, c.CreateDate,
				(c.Stacks == null ? new ProfilingStackItem[0] : c.Stacks.Select(i => ProfilingStackItem.CreateInstance(i, c.ElapsedMilliseconds)).ToArray()),
				(StackType)c.StackType, c.AdditionalInfo)).ToArray();

			if (!_stacks.Any())
				_stacks = null;

			if (dto != null)
			{
				_reportSlowness = dto.ReportSlowness;
			}
		}

		public string LatencyInfo
		{
			get { return _latencyInfo; }
		}

		public ProfilingStack[] Stacks
		{
			get { return _stacks; }
		}

		public ProfilingReportSlownessDTO[] ReportSlowness
		{
			get { return _reportSlowness; }
		}

		public bool IsFakeLatency()
		{
			return LatencyInfo == null;
		}

		public bool IsFakeStacks()
		{
			return _stacks == null;
		}
	}
}
