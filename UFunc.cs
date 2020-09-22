using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace caMon.pagemod.LCD4Passenger
{
	static public class UFunc
	{
		static public Encoding XMLEncoding { get; set; } = Encoding.Default;
		static public T LoadFromXMLFile<T>(in string path)
		{
			//ref : https://dobon.net/vb/dotnet/file/xmlserializer.html
			using (StreamReader sr = new StreamReader(path, XMLEncoding))
				return (T)(new XmlSerializer(typeof(T))).Deserialize(sr);
		}

		static public T LoadFromXMLData<T>(in string data) => (T)(new XmlSerializer(typeof(T))).Deserialize(new MemoryStream(XMLEncoding.GetBytes(data)));

		static public string ConvertToXMLData<T>(in T data) {
			MemoryStream ms = new MemoryStream();
			(new XmlSerializer(typeof(T))).Serialize(ms, data);

			return XMLEncoding.GetString(ms.ToArray());
		}
	}
}
