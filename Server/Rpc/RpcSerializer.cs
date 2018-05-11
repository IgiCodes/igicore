using IgiCore.Core.Rpc;
using Newtonsoft.Json;

namespace IgiCore.Server.Rpc
{
	public class RpcSerializer : IRpcSerializer
	{
		public string Serialize(object obj) => JsonConvert.SerializeObject(obj);

		public T Deserialize<T>(string data) => JsonConvert.DeserializeObject<T>(data);
	}
}
