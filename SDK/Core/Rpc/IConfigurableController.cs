namespace IgiCore.SDK.Core.Rpc
{
	public interface IConfigurableController<T> where T : IControllerConfiguration
	{
		T Configuration { get; set; }
	}
}
