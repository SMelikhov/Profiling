using System.Xml.Serialization;

namespace Profiling.Common
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	[XmlType(TypeName = "StackItem")]
	public sealed class ProfilingStackItemDTO
	{
		[DataMember(Order = 1)]
		[XmlAttribute("Ht")]
		public long Hints;

		[DataMember(Order = 2)]
		[XmlAttribute("St")]
		public string Stack;

		[DataMember(Order = 3)]
		[XmlElement("Ch")]
		public ProfilingStackItemDTO[] Children;
	}
}
