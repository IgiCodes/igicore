using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Commands
{
	public class CarCommand : Command
	{
		public override string Name => "car";

		public override async Task RunCommand(Player player, List<string> args)
		{
			Car car = new Car
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Hash = (uint)VehicleHash.Elegy,
				Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f },
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

			Server.Db.Cars.Add(car);
			await Server.Db.SaveChangesAsync();

			player
				.Event(RpcEvents.CarSpawn)
				.Attach(car)
				.Trigger();
		}
	}
}
