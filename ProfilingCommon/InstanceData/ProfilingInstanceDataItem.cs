using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Profiling.Common.DTO;

namespace Profiling.Common.InstanceData
{
	public sealed class ProfilingInstanceDataItem
	{
		public double ElapsedMilliseconds;
		public string Operation;
		public string SubOperation;
		public DateTime CreateDate;
		public bool HighFrequencyOperation;
		public int Count;
		public Dictionary<string, string> SystemTags { get; set; }
		public Dictionary<string, object> Values { get; set; }

		public ProfilingInstanceDataItemDTO ToDto(bool needSystemTags, bool needValues)
		{
			var dto = new ProfilingInstanceDataItemDTO
			{
				ElapsedMilliseconds = ElapsedMilliseconds,
				Operation = Operation,
				SubOperation = SubOperation,
				CreateDate = CreateDate,
				HighFrequencyOperation = HighFrequencyOperation,
				Count = Count
			};

			if (needSystemTags && SystemTags != null)
			{
				dto.SystemTags = SystemTags.ToList();
			}

			if (needValues && Values != null)
			{
				var values = Values.Select(c => new KeyValuePair<string, object>(c.Key, c.Value)).ToList();
				dto.Values = Serialize(values);
			}

			return dto;
		}

		public ProfilingInstanceDataItem()
		{
			
		} 

		public ProfilingInstanceDataItem(ProfilingInstanceDataItemDTO dto, Dictionary<string, string> systemTags)
		{
			ElapsedMilliseconds = dto.ElapsedMilliseconds;
			Operation = dto.Operation;
			SubOperation = dto.SubOperation;
			CreateDate = dto.CreateDate;
			HighFrequencyOperation = dto.HighFrequencyOperation;
			Count = dto.Count;

			if (dto.SystemTags != null)
			{
				SystemTags = dto.SystemTags.ToDictionary(c=> c.Key, c => c.Value);
			}

			if (systemTags != null)
			{
				SystemTags = systemTags;
			}

			if (dto.Values != null)
			{
				Values = Deserialize(dto.Values).ToDictionary(c=> c.Key, c => (object)c.Value);
			}
		}


		private static byte[] Serialize(List<KeyValuePair<string, object>> o)
		{
			if (o == null)
				return null;

			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, o);
				return ms.ToArray();
			}
		}

		private static List<KeyValuePair<string, object>> Deserialize(byte[] b)
		{
			if (b == null)
				return null;

			using (var ms = new MemoryStream(b))
			{
				var formatter = new BinaryFormatter();
				return (List<KeyValuePair<string, object>>)formatter.Deserialize(ms);
			}
		}

	}
}
