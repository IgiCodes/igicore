using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;

namespace IgiCore.Client.Interface
{
	public static class Nui
	{
		public static void Send(string type, object data = null) => API.SendNuiMessage(JsonConvert.SerializeObject(new
		{
			type,
			data
		}));

		public static void RegisterCallback(string type, Action<dynamic, CallbackDelegate> callback)
		{
			API.RegisterNuiCallbackType(type);

			Client.Instance.Handlers[$"__cfx_nui:{type}"] += callback; // TODO: Dispose
		}
	}
}
