using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	[XmlType(TypeName = "Latency")]
	public sealed class ProfilingLatencyDTO
	{
		[DataMember(Order = 1)]
		[XmlAttribute("Info")]
		public string LatencyInfo;

		[DataMember(Order = 2)]
		[XmlElement("St")]
		public ProfilingStackDTO[] Stacks;

		[DataMember(Order = 3)]
		[XmlElement("Rp")]
		public ProfilingReportSlownessDTO[] ReportSlowness;
	}
}
