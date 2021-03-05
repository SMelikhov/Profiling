using System.Collections.Generic;
using Profiling.Common.DTO;

namespace Profiling.Common.Sampling
{
	internal sealed class ProfilingReportSlowness
	{
		private readonly List<ProfilingReportSlownessDTO> _list = new List<ProfilingReportSlownessDTO>();

		public void Report(ProfilingReportSlownessDTO reportSlowness)
		{
			lock (_list)
			{
				_list.Add(reportSlowness);
			}
		}

		public ProfilingReportSlownessDTO[] GetProfilingReportSlownes()
		{
			ProfilingReportSlownessDTO[] result;
			lock (_list)
			{
				result = _list.ToArray();
			}
			return result;
		}

		public void Stop()
		{
			lock (_list)
			{
				_list.Clear();
			}
		}
	}
}
