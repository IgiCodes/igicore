using System;
using CitizenFX.Core;
using Newtonsoft.Json;

namespace IgiCore.Client
{
	public class ClientBase : BaseScript
	{
		public static void TriggerServerJsonEvent(string eventName, object o1) => TriggerServerEvent(eventName, JsonConvert.SerializeObject(o1));
		public static void TriggerServerJsonEvent(string eventName, object o1, object o2) => TriggerServerEvent(eventName, JsonConvert.SerializeObject(o1), JsonConvert.SerializeObject(o2));

		public void Attach(string eventName, Delegate action) => this.EventHandlers[eventName] += action;
		public void Dettach(string eventName, Delegate action) => this.EventHandlers[eventName] -= action;

		public void HandleEvent(string name, Delegate action) => this.EventHandlers[name] += action;
		public void HandleEvent<T1>(string name, Action<T1> action) => this.EventHandlers[name] += action;
		public void HandleEvent<T1, T2>(string name, Action<T1, T2> action) => this.EventHandlers[name] += action;
		public void HandleEvent<T1, T2, T3>(string name, Action<T1, T2, T3> action) => this.EventHandlers[name] += action;
		public void HandleJsonEvent<T>(string eventName, Action<T> action) => this.EventHandlers[eventName] += new Action<string>(json => action(JsonConvert.DeserializeObject<T>(json)));
		public void HandleJsonEvent<T1, T2>(string eventName, Action<T1, T2> action) => this.EventHandlers[eventName] += new Action<string, string>((j1, j2) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2)));
		public void HandleJsonEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) => this.EventHandlers[eventName] += new Action<string, string, string>((j1, j2, j3) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2), JsonConvert.DeserializeObject<T3>(j3)));
	}
}
