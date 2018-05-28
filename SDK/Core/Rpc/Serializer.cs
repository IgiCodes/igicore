using Newtonsoft.Json;

namespace IgiCore.SDK.Core.Rpc
{
	public class Serializer
	{
		public string Serialize(object obj) => JsonConvert.SerializeObject(obj);

		public T Deserialize<T>(string data) => JsonConvert.DeserializeObject<T>(data);
	}
}
