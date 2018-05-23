using System;
using System.Linq;
using System.Reflection;
using CitizenFX.Core;
using IgiCore.Client.Diagnostics;
using IgiCore.Client.Services;
using IgiCore.SDK.Client;
using JetBrains.Annotations;

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
}
