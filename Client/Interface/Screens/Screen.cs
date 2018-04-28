using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;

namespace IgiCore.Client.Interface.Screens
{
	public abstract class Screen : IDisposable
	{
		public abstract Task Render();

		public abstract Task Show();
		public abstract Task Hide();

		public virtual void Dispose()
		{
			// TODO: Deregister
		}
	}
}
