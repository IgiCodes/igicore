using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;

namespace IgiCore.Client.Rpc
{
	/// <summary>
	/// Provides event based communication with the server.
	/// </summary>
	public static class Server
	{
		/// <summary>
		/// Triggers the specified event on the server.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		public static void Trigger(string eventName) => BaseScript.TriggerServerEvent(eventName);

		/// <summary>
		/// Triggers the specified event on the server with specified payload.
		/// The payload will be serialized before being sent.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="o1">First event payload.</param>
		public static void Trigger(string eventName, object o1) => BaseScript.TriggerServerEvent(eventName, JsonConvert.SerializeObject(o1));

		/// <summary>
		/// Triggers the specified event on the server with specified payloads.
		/// The payloads will be serialized before being sent.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="o1">First event payload.</param>
		/// <param name="o2">Second event payload.</param>
		public static void Trigger(string eventName, object o1, object o2) => BaseScript.TriggerServerEvent(eventName, JsonConvert.SerializeObject(o1), JsonConvert.SerializeObject(o2));

		/// <summary>
		/// Triggers the specified event on the server with specified payloads.
		/// The payloads will be serialized before being sent.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="o1">First event payload.</param>
		/// <param name="o2">Second event payload.</param>
		/// <param name="o3">Third event payload.</param>
		public static void Trigger(string eventName, object o1, object o2, object o3) => BaseScript.TriggerServerEvent(eventName, JsonConvert.SerializeObject(o1), JsonConvert.SerializeObject(o2), JsonConvert.SerializeObject(o3));
		
		/// <summary>
		/// Triggers an action when the server sends the specified event.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="action">The action to trigger.</param>
		public static void On(string eventName, Delegate action) => Client.Instance.Handlers[eventName] += action;

		/// <summary>
		/// Triggers an action when the server sends the specified event and payload.
		/// The data payload is deserialized before the action is triggered.
		/// </summary>
		/// <typeparam name="T">First payload data type.</typeparam>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="action">The action to trigger.</param>
		public static void On<T>(string eventName, Action<T> action) => Client.Instance.Handlers[eventName] += new Action<string>(json => action(JsonConvert.DeserializeObject<T>(json)));

		/// <summary>
		/// Triggers an action when the server sends the specified event and payloads.
		/// The data payloads are deserialized before the action is triggered.
		/// </summary>
		/// <typeparam name="T1">First payload data type.</typeparam>
		/// <typeparam name="T2">Second payload data type.</typeparam>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="action">The action to trigger.</param>
		public static void On<T1, T2>(string eventName, Action<T1, T2> action) => Client.Instance.Handlers[eventName] += new Action<string, string>((j1, j2) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2)));

		/// <summary>
		/// Triggers an action when the server sends the specified event and payloads.
		/// The data payloads are deserialized before the action is triggered.
		/// </summary>
		/// <typeparam name="T1">First payload data type.</typeparam>
		/// <typeparam name="T2">Second payload data type.</typeparam>
		/// <typeparam name="T3">Third payload data type.</typeparam>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="action">The action to trigger.</param>
		public static void On<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) => Client.Instance.Handlers[eventName] += new Action<string, string, string>((j1, j2, j3) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2), JsonConvert.DeserializeObject<T3>(j3)));

		/// <summary>
		/// Triggers the specified server event and waits for the server to fire a return event.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		public static async Task Request(string eventName)
		{
			var tcs = new TaskCompletionSource<bool>();
			var handler = new Action(() => tcs.SetResult(true));

			try
			{
				Client.Instance.Handlers[eventName] += handler;

				BaseScript.TriggerServerEvent(eventName);

				await tcs.Task;
			}
			finally
			{
				Client.Instance.Handlers[eventName] -= handler;
			}
		}

		/// <summary>
		/// Triggers the specified server event and waits for the server to fire a return event with a payload.
		/// The data payload is deserialized before being returned.
		/// </summary>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <param name="eventName">Name of the event.</param>
		/// <returns></returns>
		public static async Task<T1> Request<T1>(string eventName)
		{
			var tcs = new TaskCompletionSource<string>();
			var handler = new Action<string>(r => tcs.SetResult(r));

			try
			{
				Client.Instance.Handlers[eventName] += handler;

				BaseScript.TriggerServerEvent(eventName);

				return JsonConvert.DeserializeObject<T1>(await tcs.Task);
			}
			finally
			{
				Client.Instance.Handlers[eventName] -= handler;
			}
		}

		public static async Task<Tuple<T1, T2>> Request<T1, T2>(string eventName)
		{
			var tcs = new TaskCompletionSource<Tuple<string, string>>();
			var handler = new Action<string, string>((r1, r2) => tcs.SetResult(new Tuple<string, string>(r1, r2)));
			
			try
			{
				Client.Instance.Handlers[eventName] += handler;

				BaseScript.TriggerServerEvent(eventName);

				var result = await tcs.Task;

				return new Tuple<T1, T2>(
					JsonConvert.DeserializeObject<T1>(result.Item1),
					JsonConvert.DeserializeObject<T2>(result.Item2)
				);
			}
			finally
			{
				Client.Instance.Handlers[eventName] -= handler;
			}
		}

		public static async Task<Tuple<T1, T2, T3>> Request<T1, T2, T3>(string eventName)
		{
			var tcs = new TaskCompletionSource<Tuple<string, string, string>>();
			var handler = new Action<string, string, string>((r1, r2, r3) => tcs.SetResult(new Tuple<string, string, string>(r1, r2, r3)));

			try
			{
				Client.Instance.Handlers[eventName] += handler;

				BaseScript.TriggerServerEvent(eventName);

				var result = await tcs.Task;

				return new Tuple<T1, T2, T3>(
					JsonConvert.DeserializeObject<T1>(result.Item1),
					JsonConvert.DeserializeObject<T2>(result.Item2),
					JsonConvert.DeserializeObject<T3>(result.Item3)
				);
			}
			finally
			{
				Client.Instance.Handlers[eventName] -= handler;
			}
		}
	}
}
