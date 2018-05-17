using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using IgiCore.Client.Controllers;
using IgiCore.Client.Controllers.Objects.Vehicles;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Core.Models.Objects.Vehicles;
using Newtonsoft.Json;
using VehicleDoor = IgiCore.Core.Models.Objects.Vehicles.VehicleDoor;
using VehicleDoorIndex = IgiCore.Core.Models.Objects.Vehicles.VehicleDoorIndex;
using VehicleHash = CitizenFX.Core.VehicleHash;
using VehicleSeat = IgiCore.Core.Models.Objects.Vehicles.VehicleSeat;
using VehicleWheel = IgiCore.Core.Models.Objects.Vehicles.VehicleWheel;
using VehicleWheelType = IgiCore.Core.Models.Objects.Vehicles.VehicleWheelType;
using VehicleWindow = IgiCore.Core.Models.Objects.Vehicles.VehicleWindow;
using VehicleWindowIndex = IgiCore.Core.Models.Objects.Vehicles.VehicleWindowIndex;

namespace IgiCore.Client.Services.Economy.Business.Driving
{
	public class DowntownCabService : ClientService
	{
		protected List<Ped> Peds { get; set; }

		protected List<Car> Cars { get; set; } = new List<Car>();

		public override async Task Tick()
		{
			await PedManager();
		}

		public async Task PedManager()
		{
			Vector3 position = new Vector3(901.1047f, -173.3021f, 73.07f);

			const float heading = 223f;
			if (this.Peds == null)
			{
				Client.Log("Spawning cabby");
				var pedModel = new Model(PedHash.Downtown01AFM);
				await pedModel.Request(-1);
				Ped newPed = await CitizenFX.Core.World.CreatePed(pedModel, position, heading);
				this.Peds = new List<Ped> { newPed };
				newPed.Task?.ClearAllImmediately();
			}

			foreach (Ped spawnedPed in Peds)
			{
				if (Ped.FromHandle(spawnedPed.Handle) == null)
				{
					Client.Log("Spawning cabby");
					var pedModel = new Model(PedHash.Downtown01AFM);
					await pedModel.Request(-1);
					Ped newPed = await CitizenFX.Core.World.CreatePed(pedModel, position, heading);
					this.Peds = new List<Ped> { newPed };
					newPed.Task?.ClearAllImmediately();
				}
				spawnedPed.Position = position;
				spawnedPed.Heading = heading;
				spawnedPed.Task?.StandStill(1);
				spawnedPed.AlwaysKeepTask = true;
				spawnedPed.IsInvincible = true;
				spawnedPed.IsPositionFrozen = true;
				spawnedPed.BlockPermanentEvents = true;
				spawnedPed.IsCollisionProof = false;
			}

			Ped ped = this.Peds
				.Select(p => new { ped = p, distance = p.Position.DistanceToSquared(Game.Player.Character.Position) })
				.Where(t => t.distance < 5.0F) // Nearby
				.OrderBy(t => t.distance)
				.Select(p => p.ped)
				.FirstOrDefault();

			if (ped == null) return;

			new Text($"Press M to rent a cab", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

			if (!Input.Input.IsControlJustPressed(Control.InteractionMenu)) return;

			Car car = new Car
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Hash = (uint)Core.Models.Objects.Vehicles.VehicleHash.Taxi,
				Position = new Vector3(904.05f, -183.71f, 73f),
				Seats = new List<VehicleSeat>
				{
					new VehicleSeat
					{
						Index = VehicleSeatIndex.LeftFront
					},
					new VehicleSeat
					{
						Index = VehicleSeatIndex.RightFront
					},
					new VehicleSeat
					{
						Index = VehicleSeatIndex.LeftRear
					},
					new VehicleSeat
					{
						Index = VehicleSeatIndex.RightRear
					}
				},
				Wheels = new List<VehicleWheel>
				{
					new VehicleWheel
					{
						Index = 0,
						IsBurst = false,
						Type = VehicleWheelType.Sport
					},
					new VehicleWheel
					{
						Index = 0,
						IsBurst = false,
						Type = VehicleWheelType.Sport
					},
					new VehicleWheel
					{
						Index = 0,
						IsBurst = false,
						Type = VehicleWheelType.Sport
					},
					new VehicleWheel
					{
						Index = 0,
						IsBurst = false,
						Type = VehicleWheelType.Sport
					}
				},
				Windows = new List<VehicleWindow>
				{
					new VehicleWindow
					{
						Index = VehicleWindowIndex.FrontLeftWindow,
						IsIntact = false,
						IsRolledDown = false
					},
					new VehicleWindow
					{
						Index = VehicleWindowIndex.FrontRightWindow,
						IsIntact = false,
						IsRolledDown = false
					},
					new VehicleWindow
					{
						Index = VehicleWindowIndex.BackLeftWindow,
						IsIntact = false,
						IsRolledDown = false
					},
					new VehicleWindow
					{
						Index = VehicleWindowIndex.BackRightWindow,
						IsIntact = false,
						IsRolledDown = false
					}
				},
				Doors = new List<VehicleDoor>
				{
					new VehicleDoor
						{Index = VehicleDoorIndex.FrontLeftDoor},
					new VehicleDoor
						{Index = VehicleDoorIndex.FrontRightDoor},
					new VehicleDoor
						{Index = VehicleDoorIndex.BackLeftDoor},
					new VehicleDoor
						{Index = VehicleDoorIndex.BackRightDoor},
					new VehicleDoor
						{Index = VehicleDoorIndex.Hood},
					new VehicleDoor
						{Index = VehicleDoorIndex.Trunk}
				}
			};

			Car spawnedCar = await Client.Instance.Controllers.First<VehicleController>().Create(car);

			this.Cars.Add(spawnedCar);
		}
	}
}
