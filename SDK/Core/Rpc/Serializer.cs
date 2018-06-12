using JetBrains.Annotations;
using Newtonsoft.Json;

namespace IgiCore.SDK.Core.Rpc
{
	[PublicAPI]
	public class Serializer
	{
		public string Serialize(object obj) => JsonConvert.SerializeObject(obj);

		public T Deserialize<T>(string data) => JsonConvert.DeserializeObject<T>(data);
	}
}
