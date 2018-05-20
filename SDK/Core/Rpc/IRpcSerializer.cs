namespace IgiCore.SDK.Core.Rpc
{
	public interface IRpcSerializer
	{
		string Serialize(object obj);
		T Deserialize<T>(string data);
	}
}
