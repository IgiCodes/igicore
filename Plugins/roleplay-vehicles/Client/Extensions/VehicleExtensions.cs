using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.SDK.Client.Extensions;
using Roleplay.Vehicles.Core.Models;
using Vehicle = CitizenFX.Core.Vehicle;
using VehicleColor = CitizenFX.Core.VehicleColor;
using VehicleDoor = CitizenFX.Core.VehicleDoor;
using VehicleHash = CitizenFX.Core.VehicleHash;
using VehicleLockStatus = CitizenFX.Core.VehicleLockStatus;
using VehicleSeat = CitizenFX.Core.VehicleSeat;
using VehicleWindow = CitizenFX.Core.VehicleWindow;
using VehicleWindowTint = CitizenFX.Core.VehicleWindowTint;

namespace Roleplay.Vehicles.Client.Extensions
{
	public static class VehicleExtensions
	{

		public static async Task<Vehicle> ToCitizenVehicle(this Core.Models.Vehicle vehicle)
		{
			Vehicle citizenVehicle = await World.CreateVehicle(new Model((VehicleHash)vehicle.Hash), vehicle.Position.ToVector3(), vehicle.Heading);

			citizenVehicle.BodyHealth = vehicle.BodyHealth;
			citizenVehicle.EngineHealth = vehicle.EngineHealth;
			citizenVehicle.DirtLevel = vehicle.DirtLevel;
			citizenVehicle.FuelLevel = vehicle.FuelLevel;
			citizenVehicle.OilLevel = vehicle.OilLevel;
			citizenVehicle.PetrolTankHealth = vehicle.PetrolTankHealth;
			citizenVehicle.TowingCraneRaisedAmount = vehicle.TowingCraneRaisedAmount;
			//citizenVehicle.HasAlarm = vehicle.HasAlarm;
			citizenVehicle.IsAlarmSet = vehicle.IsAlarmed;
			//citizenVehicle.HasLock = vehicle.HasLock;
			citizenVehicle.IsDriveable = vehicle.IsDriveable;
			citizenVehicle.IsEngineRunning = vehicle.IsEngineRunning;
			//citizenVehicle.HasSeatbelts = vehicle.HasSeatbelts;
			citizenVehicle.AreHighBeamsOn = vehicle.IsHighBeamsOn;
			citizenVehicle.AreLightsOn = vehicle.IsLightsOn;
			citizenVehicle.IsInteriorLightOn = vehicle.IsInteriorLightOn;
			citizenVehicle.IsSearchLightOn = vehicle.IsSearchLightOn;
			citizenVehicle.IsTaxiLightOn = vehicle.IsTaxiLightOn;
			citizenVehicle.IsLeftIndicatorLightOn = vehicle.IsLeftIndicatorLightOn;
			citizenVehicle.IsRightIndicatorLightOn = vehicle.IsRightIndicatorLightOn;
			//citizenVehicle.IsFrontBumperBrokenOff = vehicle.IsFrontBumperBrokenOff;
			//citizenVehicle.IsRearBumperBrokenOff = vehicle.IsRearBumperBrokenOff;
			citizenVehicle.IsLeftHeadLightBroken = vehicle.IsLeftHeadLightBroken;
			citizenVehicle.IsRightHeadLightBroken = vehicle.IsRightHeadLightBroken;
			citizenVehicle.IsRadioEnabled = vehicle.IsRadioEnabled;
			citizenVehicle.RoofState = vehicle.IsRoofOpen ? VehicleRoofState.Opened : VehicleRoofState.Closed;
			citizenVehicle.NeedsToBeHotwired = vehicle.NeedsToBeHotwired;
			citizenVehicle.CanTiresBurst = vehicle.CanTiresBurst;

			if (vehicle.PrimaryColor.IsCustom) citizenVehicle.Mods.CustomPrimaryColor = vehicle.PrimaryColor.CustomColor.ToCitColor();
			else citizenVehicle.Mods.PrimaryColor = (VehicleColor)vehicle.PrimaryColor.StockColor;

			if (vehicle.SecondaryColor.IsCustom) citizenVehicle.Mods.CustomSecondaryColor = vehicle.SecondaryColor.CustomColor.ToCitColor();
			else citizenVehicle.Mods.SecondaryColor = (VehicleColor)vehicle.SecondaryColor.StockColor;

			citizenVehicle.Mods.PearlescentColor = (VehicleColor)vehicle.PearescentColor;
			citizenVehicle.Mods.RimColor = (VehicleColor)vehicle.RimColor;
			citizenVehicle.Mods.TrimColor = (VehicleColor)vehicle.TrimColor;
			citizenVehicle.Mods.DashboardColor = (VehicleColor)vehicle.DashboardColor;
			citizenVehicle.Mods.NeonLightsColor = vehicle.NeonColor.ToCitColor();

			citizenVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, vehicle.NeonPositions.HasFlag(VehicleNeonPositions.Back));
			citizenVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, vehicle.NeonPositions.HasFlag(VehicleNeonPositions.Front));
			citizenVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, vehicle.NeonPositions.HasFlag(VehicleNeonPositions.Left));
			citizenVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, vehicle.NeonPositions.HasFlag(VehicleNeonPositions.Right));

			citizenVehicle.Mods.WindowTint = (VehicleWindowTint)(int)vehicle.WindowTint;
			citizenVehicle.LockStatus = (VehicleLockStatus)(int)vehicle.LockStatus;

			//citizenVehicle.RadioStation = (RadioStation)(int)vehicle.RadioStation;

			//TODO: Set vehicle Extras/Seats/Doors/Windows/Wheels/etc

			return citizenVehicle;
		}

		public static async Task<T> ToVehicle<T>(this Vehicle vehicle, Guid id = default(Guid)) where T : IVehicle, new()
		{
			if (id == default(Guid)) id = Guid.NewGuid();

			// Extras
			List<VehicleExtra> vehicleExtras = new List<VehicleExtra>();
			for (int i = 0; i < 100; i++)
			{
				if (vehicle.ExtraExists(i)) vehicleExtras.Add(new VehicleExtra { Index = i, IsOn = vehicle.IsExtraOn(i), Id = id });
			}

			// Wheels
			List<Core.Models.VehicleWheel> vehicleWheels = new List<Core.Models.VehicleWheel>();
			foreach (KeyValuePair<VehicleWheelPosition, string> wheelBoneName in VehicleWheelBones.Bones)
			{
				if (vehicle.Bones.HasBone(wheelBoneName.Value))
				{
					vehicleWheels.Add(new Core.Models.VehicleWheel
					{
						Type = (Core.Models.VehicleWheelType)(int)vehicle.Mods.WheelType,
						Position = wheelBoneName.Key,
						Index = vehicle.Wheels[(int)wheelBoneName.Key].Index
					});
				}
			}

			// Doors
			List<Core.Models.VehicleDoor> vehicleDoors = new List<Core.Models.VehicleDoor>();
			foreach (VehicleDoor vehicleDoor in vehicle.Doors)
			{
				vehicleDoors.Add(new Core.Models.VehicleDoor
				{
					Index = (Core.Models.VehicleDoorIndex)(int)vehicleDoor.Index,
					IsBroken = vehicleDoor.IsBroken,
					IsOpen = vehicleDoor.IsOpen,
					Angle = vehicleDoor.AngleRatio
				});
			}

			List<Core.Models.VehicleWindow> vehicleWindows = new List<Core.Models.VehicleWindow>();
			foreach (var value in Enum.GetValues(typeof(CitizenFX.Core.VehicleWindowIndex)))
			{
				CitizenFX.Core.VehicleWindow window = vehicle.Windows[(CitizenFX.Core.VehicleWindowIndex)value];
				vehicleWindows.Add(new Core.Models.VehicleWindow
				{
					Index = (Core.Models.VehicleWindowIndex)value,
					IsIntact = window.IsIntact,
					//TODO: Window rolled down state (has to be self-tracked)
				});
			}

			// TODO: Store player server IDs when communicating with the server and have the server assign a character
			PlayerList players = new PlayerList();
			List<Core.Models.VehicleSeat> vehicleSeats = new List<Core.Models.VehicleSeat>();
			foreach (Ped vehicleOccupant in vehicle.Occupants)
			{
				foreach (Player player in players)
				{
					if (player.Handle == vehicleOccupant.Handle)
					{
						vehicleSeats.Add(new Core.Models.VehicleSeat
						{
							Index = (VehicleSeatIndex)(int)vehicleOccupant.SeatIndex,
						});
					}
				}
			}

			VehicleNeonPositions neonPositions = VehicleNeonPositions.None;
			if (vehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Back)) neonPositions |= VehicleNeonPositions.Back;
			if (vehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Front)) neonPositions |= VehicleNeonPositions.Front;
			if (vehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Right)) neonPositions |= VehicleNeonPositions.Right;
			if (vehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Left)) neonPositions |= VehicleNeonPositions.Left;

			return new T
			{
				Id = id,
				Hash = vehicle.Model.Hash,
				Handle = vehicle.Handle,
				Position = vehicle.Position.ToPosition(),
				Heading = vehicle.Heading,
				BodyHealth = vehicle.BodyHealth,
				EngineHealth = vehicle.EngineHealth,
				DirtLevel = vehicle.DirtLevel,
				FuelLevel = vehicle.FuelLevel,
				OilLevel = vehicle.OilLevel,
				PetrolTankHealth = vehicle.PetrolTankHealth,
				PrimaryColor = new Core.Models.VehicleColor
				{
					StockColor = (VehicleStockColor)vehicle.Mods.PrimaryColor,
					CustomColor = vehicle.Mods.CustomPrimaryColor.ToColor(),
					IsCustom = vehicle.Mods.IsPrimaryColorCustom
				},
				SecondaryColor = new Core.Models.VehicleColor
				{
					StockColor = (VehicleStockColor)vehicle.Mods.SecondaryColor,
					CustomColor = vehicle.Mods.CustomSecondaryColor.ToColor(),
					IsCustom = vehicle.Mods.IsSecondaryColorCustom
				},
				PearescentColor = (VehicleStockColor)vehicle.Mods.PearlescentColor,
				RimColor = (VehicleStockColor)vehicle.Mods.RimColor,
				TrimColor = (VehicleStockColor)vehicle.Mods.TrimColor,
				DashboardColor = (VehicleStockColor)vehicle.Mods.DashboardColor,
				NeonColor = vehicle.Mods.NeonLightsColor.ToColor(),
				NeonPositions = neonPositions,
				TireSmokeColor = vehicle.Mods.TireSmokeColor.ToColor(),
				WindowTint = (Core.Models.VehicleWindowTint)vehicle.Mods.WindowTint,
				Class = (Core.Models.VehicleClass)vehicle.ClassType,
				LockStatus = (Core.Models.VehicleLockStatus)vehicle.LockStatus,
				CanTiresBurst = vehicle.CanTiresBurst,
				NeedsToBeHotwired = vehicle.NeedsToBeHotwired,
				IsVehicleConvertible = vehicle.IsConvertible,
				HasRoof = vehicle.HasRoof,
				IsRoofOpen = vehicle.RoofState != VehicleRoofState.Closed,
				IsRightHeadLightBroken = vehicle.IsRightHeadLightBroken,
				IsLeftHeadLightBroken = vehicle.IsLeftHeadLightBroken,
				IsRearBumperBrokenOff = vehicle.IsRearBumperBrokenOff,
				IsFrontBumperBrokenOff = vehicle.IsFrontBumperBrokenOff,
				IsTaxiLightOn = vehicle.IsTaxiLightOn,
				IsSearchLightOn = vehicle.IsSearchLightOn,
				//IsInteriorLightOn = vehicle.IsInteriorLightOn,  // < THIS WILL CRASH THE GAME BECAUSE FUCK YOU THAT'S WHY!
				IsLightsOn = vehicle.AreLightsOn,
				IsHighBeamsOn = vehicle.AreHighBeamsOn,
				IsEngineRunning = vehicle.IsEngineRunning,
				IsDriveable = vehicle.IsDriveable,
				IsAlarmed = vehicle.IsAlarmSet,
				IsAlarmSounding = vehicle.IsAlarmSounding,
				LicensePlate = vehicle.Mods.LicensePlate,
				Extras = vehicleExtras,
				Wheels = vehicleWheels,
				Doors = vehicleDoors,
				Seats = vehicleSeats,
				Windows = vehicleWindows
			};
		}

		public static async Task<Car> ToCar(this Vehicle vehicle) => await vehicle.ToVehicle<Car>();

		public static void ToggleEngine(this Vehicle vehicle, bool value, bool instant, bool otherwise) => API.SetVehicleEngineOn(vehicle.Handle, value, instant, otherwise);
	}
}
