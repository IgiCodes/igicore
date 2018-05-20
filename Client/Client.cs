using System;
using System.Linq;
using System.Reflection;
using CitizenFX.Core;
using IgiCore.SDK.Client;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Core.Diagnostics;
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

		/// <summary>
		/// Primary client entrypoint.
		/// Initializes a new instance of the <see cref="Client"/> class.
		/// </summary>
		public Client()
		{
			// -- INIT
			Debug.WriteLine("[CLIENT] Init");

			// Singleton
			Instance = this;

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly.FullName.StartsWith("EntityFramework")) continue; // HACK: EF doesn't load properly
				if (assembly.GetCustomAttribute<ClientPluginAttribute>() == null) continue;

				Debug.WriteLine(assembly.GetName().Name);

				foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Service))))
				{
					Debug.WriteLine("    " + type.FullName);

					var controller = (Service)Activator.CreateInstance(type, new Logger(), new EventsManager()); // TODO: Args - DI?

					this.Tick += controller.Tick;
				}
			}
		}
	}

	class EventsManager : IEventsManager
	{

	}

	class Logger : ILogger
	{
		public void Log(string message)
		{
			Debug.WriteLine($"{message}");
		}

		public void Error(Exception exception)
		{
			Debug.WriteLine($"ERROR: {exception.Message}");
		}
	}
}
