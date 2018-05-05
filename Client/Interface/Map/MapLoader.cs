using System.Collections.Generic;
using CitizenFX.Core.Native;

namespace IgiCore.Client.Interface.Map
{
	public abstract class MapLoader
	{
		protected abstract IEnumerable<string> Ipls { get; }

		public void Load()
		{
			foreach (var ipl in this.Ipls) API.RequestIpl(ipl);
		}

		public void Unload()
		{
			foreach (var ipl in this.Ipls) API.RemoveIpl(ipl);
		}
	}
}
