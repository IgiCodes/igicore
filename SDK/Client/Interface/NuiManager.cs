using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.SDK.Core.Rpc;


namespace IgiCore.SDK.Client.Interface
{
	public class NuiManager : INuiManager
	{
		private readonly EventHandlerDictionary events;

		public NuiManager(EventHandlerDictionary events) { this.events = events; }

		public void Send(string type, object data = null) => API.SendNuiMessage(new Serializer().Serialize(new
		{
			type,
			data
		}));

		public void Attach(string type, Action<dynamic, CallbackDelegate> callback)
		{
			API.RegisterNuiCallbackType(type);
			
			this.events[$"__cfx_nui:{type}"] += callback; // TODO: Dispose
		}
	}
}
