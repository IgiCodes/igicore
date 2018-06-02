using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Client.Controllers;
using IgiCore.Client.Controllers.Objects.Vehicles;
using IgiCore.Client.Controllers.Player;
using IgiCore.Client.Interface;
using IgiCore.Client.Interface.Hud;
using IgiCore.Client.Interface.Menu;
using IgiCore.Client.Managers;
using IgiCore.Client.Managers.World;
using IgiCore.Client.Services;
using IgiCore.Client.Services.AI;
using IgiCore.Client.Services.Economy.Banking;
using IgiCore.Client.Services.Economy.Business.Driving;
using IgiCore.Client.Services.Player;
using IgiCore.Client.Services.Vehicle;
using IgiCore.Client.Services.World;
using IgiCore.Core.Controllers;
using JetBrains.Annotations;
using Debug = CitizenFX.Core.Debug;
using Screen = CitizenFX.Core.UI.Screen;

namespace IgiCore.Client
{
	[PublicAPI]
	public class Client : BaseScript
	{
		/// <summary>
		/// Gets or sets the global singleton instance reference.
		/// </summary>
		/// <value>
		/// The singleton <see cref="Client"/> instance.
		/// </value>
		public static Client Instance { get; protected set; }

		public ControllerRegistry Controllers { get; protected set; }
		public ManagerRegistry Managers { get; protected set; }
		public ServiceRegistry Services { get; protected set; }		

		public EventHandlerDictionary Handlers => this.EventHandlers;

		/// <summary>
		/// Primary client entrypoint.
		/// Initializes a new instance of the <see cref="Client"/> class.
		/// </summary>
		public Client()
		{
			// -- INIT
			Log("Init");

			// Singleton
			Instance = this;

			this.Controllers = new ControllerRegistry
			{
				new ClientController(),
				new UserController(),
				new CharacterController(),
				new VehicleController(),
			};

			this.Managers = new ManagerRegistry
			{
				new HudManager(), // Resets and hides all HUD elements
				new MapManager(), // Loads IPLs and blips
				new MenuManager() // Set initial menu options
			};

			this.Services = new ServiceRegistry
			{
				new VehicleService(), // Vehicle tracking service
				new VehicleRollService(), // Disable rolling cars back over
				new PlayerDeathService(), // Knock down players rather than death
				new PlayerIdleService(), // Kick idle players
				new AutosaveService(),
				new PedFilterService(), // Block blacklisted peds
				new AiPoliceService(), // Disable AI police
				new PlayerIndicatorService(), // Show nearby players
				new DateTimeService(), // Set the date and time
				new BlackoutService(), // Allow city blackouts
				new AtmService(), // Add ATMs
                new BranchService(), // Add Bank Tellers
				new DowntownCabService()
			};

			this.Services.Initialize(); // Attach handlers

			// -- SERVICE EVENTS

			// Player Death Service
			this.Services.First<PlayerDeathService>().OnDowned += (s, e) =>
			{
				UI.ShowNotification("Downed");
				if (this.LocalPlayer.Character.Weapons.Current.Group != WeaponGroup.Unarmed) this.LocalPlayer.Character.Weapons.Remove(this.LocalPlayer.Character.Weapons.Current);
			};

			this.Services.First<PlayerDeathService>().OnRevived += (s, e) => Screen.ShowNotification("Revived");

			this.Controllers.First<ClientController>().Startup();
		}

		[Conditional("DEBUG")]
		public static void Log(string message) => Debug.Write($"{DateTime.Now:s} [CLIENT]: {message}{Environment.NewLine}");

		public void AttachTickHandler(Func<Task> task) => this.Tick += task;

		public void DettachTickHandler(Func<Task> task) => this.Tick -= task;

	}
}
