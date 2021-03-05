using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Profiling.Common.DTO
{
	[Serializable]
	[DataContract(Namespace = "http://schemas.test.com/2021")]
	[XmlType(TypeName = "Stack")]
	public sealed class ProfilingInstanceDataItemDTO
	{
		[DataMember(Order = 1)]
		[XmlAttribute("Ms")]
		public double ElapsedMilliseconds;

		[DataMember(Order = 2)]
		[XmlAttribute("Op")]
		public string Operation;

		[DataMember(Order = 3)]
		[XmlAttribute("Sub")]
		public string SubOperation;

		[DataMember(Order = 4)]
		[XmlAttribute("Dt")]
		public DateTime CreateDate;

		[DataMember(Order = 5)]
		[XmlAttribute("Hi")]
		public bool HighFrequencyOperation;

		[DataMember(Order = 6)]
		[XmlAttribute("Tp")]
		public int Count;

		[DataMember(Order = 7)]
		[XmlAttribute("St")]
		public List<KeyValuePair<string, string>> SystemTags;

		[DataMember(Order = 8)]
		[XmlAttribute("Val")]
		public byte[] Values;

	}
}
