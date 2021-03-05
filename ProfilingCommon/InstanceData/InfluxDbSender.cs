using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Profiling.Common.Util;

namespace Profiling.Common.InstanceData
{
	public class InfluxDbSender
	{
		private readonly string _url;
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public const string DataBase = "DataBase";
		public const string SystemTagProcess = "Process";
		public const string SystemTagHost = "Host";

		public const string UserName = "UserName";

		private const string TableName = "Profiling";
		private readonly string _dataBase;
		private readonly string _tableName;
		private readonly List<KeyValuePair<string, string>> _tagKeysOrdered;

		private static readonly Dictionary<string, string> SystemTags = new Dictionary<string, string>
		{
			{ SystemTagHost, Environment.MachineName},
			{ SystemTagProcess, ParseProcessName(Process.GetCurrentProcess().ProcessName) },
			{ UserName, null },

		};

		private static readonly Dictionary<Type, Func<object, string>> Formatters = new Dictionary<Type, Func<object, string>>
				{
						{ typeof(sbyte), FormatInteger },
						{ typeof(sbyte?), FormatInteger },
						{ typeof(byte), FormatInteger },
						{ typeof(byte?), FormatInteger },
						{ typeof(short), FormatInteger },
						{ typeof(short?), FormatInteger },
						{ typeof(ushort), FormatInteger },
						{ typeof(ushort?), FormatInteger },
						{ typeof(int), FormatInteger },
						{ typeof(int?), FormatInteger },
						{ typeof(uint), FormatInteger },
						{ typeof(uint?), FormatInteger },
						{ typeof(long), FormatInteger },
						{ typeof(long?), FormatInteger },
						{ typeof(ulong), FormatInteger },
						{ typeof(ulong?), FormatInteger },
						{ typeof(float), FormatFloat },
						{ typeof(float?), FormatFloat },
						{ typeof(double), FormatFloat },
						{ typeof(double?), FormatFloat },
						{ typeof(decimal), FormatFloat },
						{ typeof(decimal?), FormatFloat },
						{ typeof(bool), FormatBoolean },
						{ typeof(bool?), FormatBoolean },
						{ typeof(TimeSpan), FormatTimespan },
						{ typeof(TimeSpan?), FormatTimespan }
				};

		public InfluxDbSender(string url, string userName = null, string dataBase = null, string tableName = null)
		{
			_url = url;

			if (!string.IsNullOrEmpty(userName))
			{
				SystemTags[UserName] = userName;
			}

			_tagKeysOrdered = SystemTags.OrderBy(p => p.Key, StringComparer.Ordinal).ToList();

			_dataBase = dataBase ?? DataBase;
			_tableName = tableName ?? TableName;
		}

		public void SaveToInfluxDb(List<ProfilingInstanceDataItem> items)
		{
			if(!items.Any())
				return;

			var counts = items.Where(c => c.HighFrequencyOperation)
				.GroupBy(g => new Tuple<string, string>(g.Operation, g.SubOperation))
				.ToDictionary(k => k.Key, value => new AggregatedInfluxItem(value.Sum(c => c.Count), value.Sum(c => c.ElapsedMilliseconds)));

			using (var client = new WebClient())
			{
				foreach (var group in items.SplitBy(100))
				{
					Send(client, group, counts);
				}
			}
		}

		private void Send(WebClient client, IEnumerable<ProfilingInstanceDataItem> group, Dictionary<Tuple<string, string>, AggregatedInfluxItem> counts)
		{
			client.UploadString(string.Format("{0}/write?db={1}", _url, _dataBase), Serialize(group, counts));
		}

		private string Serialize(IEnumerable<ProfilingInstanceDataItem> items, Dictionary<Tuple<string, string>, AggregatedInfluxItem> counts)
		{
			var builder = new StringBuilder(50 * items.Count());

			foreach (var item in items)
			{
				AggregatedInfluxItem influxItem;
				if (counts.TryGetValue(new Tuple<string, string>(item.Operation, item.SubOperation), out influxItem))
				{
					if(influxItem.IsReported)
						continue;
				}

				builder.Append(_tableName);
				AppendName(builder, "Id", item.Operation, false);
				if (!string.IsNullOrEmpty(item.SubOperation))
					AppendName(builder, "SubOperation", item.SubOperation, true);

				if (item.SystemTags != null)
				{
					string processValue;
					if (item.SystemTags.TryGetValue(SystemTagProcess, out processValue))
					{
						item.SystemTags[SystemTagProcess] = ParseProcessName(processValue);
					}
				}

				foreach (var property in _tagKeysOrdered)
				{
					string value;
					if (item.SystemTags == null || !item.SystemTags.TryGetValue(property.Key, out value) || string.IsNullOrEmpty(value))
					{
						value = property.Value;
					}
					if(!string.IsNullOrEmpty(value))
						AppendName(builder, property.Key, value, true);
				}

				builder.Append(' ');

				if (influxItem?.ElapsedMilliseconds > 0)
				{
					AppendValue(builder, "Elapsed", influxItem.ElapsedMilliseconds, false);
				}
				else
				{
					AppendValue(builder, "Elapsed", item.ElapsedMilliseconds, false);
				}

				builder.Append(',');
				if (influxItem?.Count > 0)
				{
					AppendValue(builder, "Count", influxItem.Count, false);
				}
				else
				{
					AppendValue(builder, "Count", item.Count, false);					
				}

				if (item.Values != null)
				{
					foreach (var property in item.Values)
					{
						builder.Append(',');
						AppendValue(builder, property.Key, property.Value, true);
					}
				}

				builder.Append(' ');

				builder.Append(GetUnixTimeStampForPost(item.CreateDate));
				builder.Append('\n');

				influxItem?.SetReported();
			}
			return builder.ToString();
		}

		private static void AppendName(StringBuilder builder, string name, string value, bool needEscapeName)
		{
			builder.Append(",");
			builder.Append(needEscapeName ? EscapeName(name) : name);
			builder.Append("=");
			builder.Append(needEscapeName ? EscapeName(value) : value);
		}

		private static void AppendValue(StringBuilder builder, string name, object value, bool needEscapeName)
		{
			builder.Append(needEscapeName ? EscapeName(name) : name);
			builder.Append("=");
			builder.Append(FormatValue(value));
		}

		private static string EscapeName(string nameOrKey)
		{
			return nameOrKey.Replace("=", "\\=").Replace(" ", "\\ ").Replace(",", "\\,").Replace("\n", "\\ ").Replace("\r", "\\ ");
		}

		private static string FormatValue(object value)
		{
			var v = value ?? string.Empty;
			Func<object, string> format;
			if (Formatters.TryGetValue(v.GetType(), out format))
			{
				return format(v);
			}

			return FormatString(EscapeValue(v.ToString()));
		}

		private static string FormatInteger(object i)
		{
			return ((IFormattable)i).ToString(null, CultureInfo.InvariantCulture);
		}

		private static string FormatFloat(object f)
		{
			return ((IFormattable)f).ToString(null, CultureInfo.InvariantCulture);
		}

		private static string FormatTimespan(object ts)
		{
			return ((TimeSpan)ts).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
		}

		private static string FormatBoolean(object b)
		{
			return ((bool)b) ? "t" : "f";
		}

		private static string FormatString(string s)
		{
			return "\"" + s.Replace("\"", "\\\"") + "\"";
		}

		private static string EscapeValue(string s)
		{
			const int maxLen = 500;
			const int lineLen = 250;

			if (s == null)
				return null;

			if (s.Length < lineLen)
				return s;

			var restricted = s.Length > maxLen ? s.Substring(0, maxLen) : s;

			var lines = restricted.Split('\n');
			var result = new StringBuilder();
			foreach (var line in lines)
			{
				result.Append(line.Length > lineLen ? line.Insert(lineLen / 2, Environment.NewLine) : line);
			}

			return result.ToString();
		}

		private static string GetUnixTimeStampForPost(DateTime utsTime)
		{
			var t = utsTime - UnixEpoch;
			return ((long)(t.TotalMilliseconds * 1000000L)).ToString(CultureInfo.InvariantCulture);
		}

		private static string ParseProcessName(string processName)
		{
			switch (processName)
			{

				default:
					return processName;
			}
		}
	}
	//SELECT count("Elapsed")*1000/$__interval_ms FROM "Profiling" WHERE("Id" = 'SaveBaskets' AND "Host" =~ /^$Host$/) AND $timeFilter GROUP BY time($__interval) fill(0)

//SELECT sum("Count")*1000/$__interval_ms as count FROM "Profiling" WHERE("Id" = 'SaveBaskets' AND "Host" =~ /^$Host$/) AND $timeFilter GROUP BY time($interval) fill(0)
}
