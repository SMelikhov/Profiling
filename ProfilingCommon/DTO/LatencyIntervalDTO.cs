using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	[XmlType(TypeName = "LatencyInterval")]
	public sealed class LatencyIntervalDTO
	{
		[DataMember(Order = 1)]
		[XmlAttribute("A0")]
		public int Above50;

		[DataMember(Order = 2)]
		[XmlAttribute("A1")]
		public int Above100;

		[DataMember(Order = 3)]
		[XmlAttribute("A3")]
		public int Above300;

		[DataMember(Order = 4)]
		[XmlAttribute("A5")]
		public int Above500;

		[DataMember(Order = 5)]
		[XmlAttribute("A7")]
		public int Above700;

		[DataMember(Order = 6)]
		[XmlAttribute("A10")]
		public int Above1000;

		[DataMember(Order = 7)]
		[XmlAttribute("A20")]
		public int Above2000;

		[DataMember(Order = 8)]
		[XmlAttribute("A30")]
		public int Above3000;

		[DataMember(Order = 9)]
		[XmlAttribute("A50")]
		public int Above5000;

		[DataMember(Order = 10)]
		[XmlAttribute("A100")]
		public int Above10000;

		[DataMember(Order = 11)]
		[XmlAttribute("A200")]
		public int Above20000;

		[DataMember(Order = 12)]
		[XmlAttribute("A600")]
		public int Above60000;

		[DataMember(Order = 120)]
		[XmlAttribute("A210")]
		public int Below50;
	}
}
