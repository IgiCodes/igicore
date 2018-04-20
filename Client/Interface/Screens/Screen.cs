using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;

namespace IgiCore.Client.Interface.Screens
{
	public abstract class Screen : IDisposable
	{
		public abstract Task Show();
		public abstract Task Hide();
		public abstract Task Render();

		protected void SendNuiMessage(string type, object data = null)
		{
			API.SendNuiMessage(JsonConvert.SerializeObject(new
			{
				type,
				data
			}));
		}

		protected void RegisterNuiCallback(string type, Action<dynamic> callback)
		{
			API.RegisterNuiCallbackType(type);

			Client.Instance.HandleEvent($"__cfx_nui:{type}", callback); // TODO: Dispose
		}

		protected void RegisterNuiCallback(string type, Action<dynamic, CallbackDelegate> callback)
		{
			API.RegisterNuiCallbackType(type);

			Client.Instance.HandleEvent($"__cfx_nui:{type}", callback); // TODO: Dispose
		}

		public virtual void Dispose()
		{
			// TODO: Deregister
		}
	}
}
