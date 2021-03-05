using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	[XmlType(TypeName = "Stack")]
	public sealed class ProfilingStackDTO
	{
		[DataMember(Order = 1)]
		[XmlAttribute("Ms")]
		public long ElapsedMilliseconds;

		[DataMember(Order = 2)]
		[XmlElement("St")]
		public ProfilingStackItemDTO[] Stacks;

		[DataMember(Order = 3)]
		[XmlAttribute("Dt")]
		public DateTime CreateDate;

		[DataMember(Order = 4)]
		[XmlAttribute("Tp")]
		public int StackType;

		[DataMember(Order = 5)]
		[XmlAttribute("Info")]
		public string AdditionalInfo;
	}
}
