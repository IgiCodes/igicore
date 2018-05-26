using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Client.Diagnostics;
using IgiCore.Client.Services;
using IgiCore.SDK.Client;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace IgiCore.Client
{
	[UsedImplicitly]
	public class Client : BaseScript
	{
		/// <summary>
		/// Gets or sets the global singleton instance reference.
		/// </summary>
		/// <value>
		/// The singleton <see cref="Client"/> instance.
		/// </value>
		public static Client Instance { get; private set; }

		public EventHandlerDictionary Handlers => this.EventHandlers;

		public static Logger Logger => new Logger();

		public EventsManager Events => new EventsManager();

		public ServiceManager Services => new ServiceManager();

		/// <summary>
		/// Primary client entrypoint.
		/// Initializes a new instance of the <see cref="Client"/> class.
		/// </summary>
		public Client()
		{
			Logger.Log("Init");

			// Singleton
			Instance = this;




			var test = new OutboundMessage
			{
				Event = "test",
				Payloads = new List<string>
				{
					JsonConvert.SerializeObject(true)
				}
			};

			Logger.Log(test.Event);
			TriggerServerEvent(test.Event, JsonConvert.SerializeObject(test));


			var test2 = new OutboundMessage
			{
				Event = "test2",
				Payloads = new List<string>
				{
					JsonConvert.SerializeObject(true),
					JsonConvert.SerializeObject(DateTime.UtcNow),
				}
			};

			Logger.Log(test2.Event);
			TriggerServerEvent(test2.Event, JsonConvert.SerializeObject(test2));


			var test3 = new OutboundMessage
			{
				Event = "test3"
			};

			Logger.Log(test3.Event);
			TriggerServerEvent(test3.Event, JsonConvert.SerializeObject(test3));





			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly.GetCustomAttribute<ClientPluginAttribute>() == null) continue;

				Logger.Log(assembly.GetName().Name);

				foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Service))))
				{
					Logger.Log("    " + type.FullName);

					var service = Activator.CreateInstance(type, new Logger(), this.Events) as Service;
					this.Tick += service.Tick;

					//this.Services.Add(service);
				}
			}

			Logger.Log("Done");
		}
	}

	public class OutboundMessage
	{
		public int Source { get; set; } = Game.Player.ServerId;

		public string Event { get; set; }

		public List<string> Payloads { get; set; } = new List<string>();

		public DateTime Sent { get; set; } = DateTime.UtcNow;
	}
}
