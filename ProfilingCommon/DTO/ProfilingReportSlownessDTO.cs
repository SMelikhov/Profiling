using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	[XmlType(TypeName = "Report")]
	public sealed class ProfilingReportSlownessDTO
	{
		[DataMember(Order = 1)]
		[XmlAttribute("Ms")]
		public string Message;

		[DataMember(Order = 2)]
		[XmlAttribute("Cd")]
		public DateTime CreateDate;
	}
}
