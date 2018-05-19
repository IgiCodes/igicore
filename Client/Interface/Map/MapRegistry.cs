using IgiCore.Core.Utility;

namespace IgiCore.Client.Interface.Map
{
	public class MapRegistry : Registry<MapLoader>
	{
		public void Load()
		{
			foreach (var loader in this.Items) loader.Load();
		}

		public void Unload()
		{
			foreach (var loader in this.Items) loader.Unload();
		}
	}
}
