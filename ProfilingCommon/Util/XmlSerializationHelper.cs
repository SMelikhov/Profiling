using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace Profiling.Common.Util
{
	internal static class XmlSerializationHelper
	{
		public static string Serialize<T>(T value, Formatting formatting = Formatting.Indented, bool omitXmlDeclaration = false)
		{
			return Serialize<T>(value, false, formatting, omitXmlDeclaration);
		}

		public static string Serialize<T>(T value, bool withError, Formatting formatting, bool omitXmlDeclaration)
		{
			string result = null;
			var success = TrySerialize(value, out result, formatting, omitXmlDeclaration);

			if (!withError && !success)
			{
				result = null;
			}

			return result;
		}

		public static bool TrySerialize<T>(T value, out string output, Formatting formatting, bool omitXmlDeclaration)
		{
			bool serialized;

			try
			{
				var sb = new StringBuilder();
				var serializer = new XmlSerializer(typeof(T));


				var settings = new XmlWriterSettings
				{
					Encoding = Encoding.Unicode,
					OmitXmlDeclaration = omitXmlDeclaration,
					Indent = formatting == Formatting.Indented
				};

				using (var xmlWriter = XmlWriter.Create(sb, settings))
				{
					serializer.Serialize(xmlWriter, value);
				}

				output = sb.ToString();
				serialized = true;
			}
			catch (Exception ex)
			{
				output = ex.ToString();
				serialized = false;
			}

			return serialized;
		}

		public static T Deserialize<T>(string xml)
		{
			using (StringReader reader = new StringReader(xml))
			{
				var TType = typeof(T);
				XmlSerializer serializer = new XmlSerializer(TType);
				var items = serializer.Deserialize(reader);
				if (items == null)
				{
					return default(T);
				}

				if (TType != items.GetType() && typeof(CollectionBase).IsAssignableFrom(TType))
				{
					var coll = (IList)Activator.CreateInstance(TType);
					foreach (var item in (IList)items)
					{
						coll.Add(item);
					}
					return (T)coll;
				}
				return (T)items;
			}
		}
	}
}
