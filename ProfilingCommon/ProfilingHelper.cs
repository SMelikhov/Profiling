using System.Diagnostics;
using System.Linq;
using System.Text;
using Profiling.Common.DTO;
using Profiling.Common.Util;

namespace Profiling.Common
{
	public static class ProfilingHelper
	{
		public static int GetParentHash(int operation)
		{
			return GetParentHash(operation, 0, null, false, 0);
		}

		public static bool NeedSave(ProfilingAggregatedItemDTO[] toSave, ProfilingLatencyDTO latency)
		{
			return (toSave.Any() || (latency != null && latency.Stacks != null && latency.Stacks.Any()));
		}

		public static int GetParentHash(int operation, int entity, string additionalInfo, bool isUi, int profilingLevel)
		{
			unchecked
			{
				int result = (int)operation;
				result = (result * 397) ^ (int)entity;
				result = (result * 397) ^ profilingLevel;
				result = (result * 397) ^ (isUi ? 10 : 50);
				if (additionalInfo != null)
				{
					result = (result * 397) ^ additionalInfo.GetHashCode();
				}
				return result;
			}
		}

		public static MemoryInfoDTO GetMemoryInfo()
		{
			using (var process = Process.GetCurrentProcess())
			{
				return new MemoryInfoDTO
				{
					VirtualMemorySize64 = process.VirtualMemorySize64,
					PrivateBytes64 = process.PrivateMemorySize64,
					WorkingSet64 = process.WorkingSet64,
					Threads = process.Threads.Count,
				};
			}
		}

		public static string SerializeProfilingLatency(ProfilingLatencyDTO value)
		{
			if (value == null)
				return null;

			try
			{
				string result = XmlSerializationHelper.Serialize<ProfilingLatencyDTO>(value);

				var compressed = ConvertHelper.Compress(Encoding.Default.GetBytes(result));
				var base64 = ConvertHelper.base64_encode(compressed);

				return base64;
			}
			catch { }
			return null;
		}

		public static ProfilingLatencyDTO DeserializeProfilingLatency(string value)
		{
			if (string.IsNullOrEmpty(value))
				return null;

			ProfilingLatencyDTO obj = null;
			try
			{
				var compressed = ConvertHelper.base64_decode(value);

				var decompressed = ConvertHelper.Decompress(compressed);
				var xml = Encoding.Default.GetString(decompressed);


				obj = XmlSerializationHelper.Deserialize<ProfilingLatencyDTO>(xml);
			}
			catch { }
			return obj;
		}


	}
}
