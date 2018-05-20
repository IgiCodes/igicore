using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Models;
using IgiCore.Core.Models.Objects.Vehicles;
using Model = CitizenFX.Core.Model;
using RadioStation = CitizenFX.Core.RadioStation;
using Vehicle = CitizenFX.Core.Vehicle;
using VehicleColor = CitizenFX.Core.VehicleColor;
using VehicleHash = CitizenFX.Core.VehicleHash;
using VehicleLockStatus = CitizenFX.Core.VehicleLockStatus;
using VehicleWindowTint = CitizenFX.Core.VehicleWindowTint;

namespace IgiCore.Client.Extensions
{
	public static class VehicleExtensions
	{
		public static async Task<Vehicle> ToCitizenVehicle(this Core.Models.Objects.Vehicles.Vehicle vehicle)
		{
			Vehicle citizenVehicle = await World.CreateVehicle(new Model((VehicleHash)vehicle.Hash), vehicle.Position, vehicle.Heading);

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

			if (vehicle.PrimaryColor.IsCustom) citizenVehicle.Mods.CustomPrimaryColor = vehicle.PrimaryColor.CustomColor;
			else citizenVehicle.Mods.PrimaryColor = (VehicleColor)vehicle.PrimaryColor.StockColor;

			if (vehicle.SecondaryColor.IsCustom) citizenVehicle.Mods.CustomSecondaryColor = vehicle.SecondaryColor.CustomColor;
			else citizenVehicle.Mods.SecondaryColor = (VehicleColor)vehicle.SecondaryColor.StockColor;

			citizenVehicle.Mods.PearlescentColor = (VehicleColor)vehicle.PearescentColor;
			citizenVehicle.Mods.RimColor = (VehicleColor)vehicle.RimColor;
			citizenVehicle.Mods.TrimColor = (VehicleColor)vehicle.TrimColor;
			citizenVehicle.Mods.DashboardColor = (VehicleColor)vehicle.DashboardColor;
			citizenVehicle.Mods.NeonLightsColor = vehicle.NeonColor;

			//citizenVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, vehicle.NeonPositions.HasFlag(VehicleNeonPositions.Back));
			//citizenVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, vehicle.NeonPositions.HasFlag(VehicleNeonPositions.Front));
			//citizenVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, vehicle.NeonPositions.HasFlag(VehicleNeonPositions.Left));
			//citizenVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, vehicle.NeonPositions.HasFlag(VehicleNeonPositions.Right));

			citizenVehicle.Mods.WindowTint = (VehicleWindowTint)(int)vehicle.WindowTint;
			citizenVehicle.LockStatus = (VehicleLockStatus)(int)vehicle.LockStatus;
			citizenVehicle.RadioStation = (RadioStation)(int)vehicle.RadioStation;
			//citizenVehicle.ClassType = (CitizenFX.Core.VehicleClass)(int)vehicle.Class;
			citizenVehicle.Heading = vehicle.Heading;

			return citizenVehicle;
		}

		public static async Task<Core.Models.Objects.Vehicles.Vehicle> ToVehicle(this CitizenFX.Core.Vehicle vehicle, Guid id = default(Guid))
		{
			if (id == default(Guid)) id = Guid.NewGuid();

			//List<VehicleExtra> vehicleExtras = new List<VehicleExtra>();
			//for (int i = 0; i < 100; i++)
			//{
			//	if (vehicle.ExtraExists(i)) vehicleExtras.Add(new VehicleExtra { Index = i, IsOn = vehicle.IsExtraOn(i), Id = id });
			//}

			VehicleNeonPositions neonPositions = VehicleNeonPositions.None;
			if (vehicle.Mods.HasNeonLight(VehicleNeonLight.Back)) neonPositions |= VehicleNeonPositions.Back;
			if (vehicle.Mods.HasNeonLight(VehicleNeonLight.Front)) neonPositions |= VehicleNeonPositions.Front;
			if (vehicle.Mods.HasNeonLight(VehicleNeonLight.Right)) neonPositions |= VehicleNeonPositions.Right;
			if (vehicle.Mods.HasNeonLight(VehicleNeonLight.Left)) neonPositions |= VehicleNeonPositions.Left;

			return new Core.Models.Objects.Vehicles.Vehicle
			{
				Id = id,
				Hash = vehicle.Model.Hash,
				Handle = vehicle.Handle,
				Position = vehicle.Position,
				Heading = vehicle.Heading,
				BodyHealth = vehicle.BodyHealth,
				EngineHealth = vehicle.EngineHealth,
				DirtLevel = vehicle.DirtLevel,
				FuelLevel = vehicle.FuelLevel,
				OilLevel = vehicle.OilLevel,
				PetrolTankHealth = vehicle.PetrolTankHealth,
				PrimaryColor = new Core.Models.Objects.Vehicles.VehicleColor
				{
					StockColor = (VehicleStockColor)vehicle.Mods.PrimaryColor,
					CustomColor = vehicle.Mods.CustomPrimaryColor,
					IsCustom = vehicle.Mods.IsPrimaryColorCustom
				},
				SecondaryColor = new Core.Models.Objects.Vehicles.VehicleColor
				{
					StockColor = (VehicleStockColor)vehicle.Mods.SecondaryColor,
					CustomColor = vehicle.Mods.CustomSecondaryColor,
					IsCustom = vehicle.Mods.IsSecondaryColorCustom
				},
				PearescentColor = (VehicleStockColor)vehicle.Mods.PearlescentColor,
				RimColor = (VehicleStockColor)vehicle.Mods.RimColor,
				TrimColor = (VehicleStockColor)vehicle.Mods.TrimColor,
				DashboardColor = (VehicleStockColor)vehicle.Mods.DashboardColor,
				NeonColor = vehicle.Mods.NeonLightsColor,
				NeonPositions = neonPositions,
				TireSmokeColor = vehicle.Mods.TireSmokeColor,
				WindowTint = (Core.Models.Objects.Vehicles.VehicleWindowTint)vehicle.Mods.WindowTint,
				Class = (Core.Models.Objects.Vehicles.VehicleClass)vehicle.ClassType,
				LockStatus = (Core.Models.Objects.Vehicles.VehicleLockStatus)vehicle.LockStatus,
				CanTiresBurst = vehicle.CanTiresBurst,
				NeedsToBeHotwired = vehicle.NeedsToBeHotwired,
				IsRoofOpen = vehicle.RoofState != VehicleRoofState.Closed,
				IsRightHeadLightBroken = vehicle.IsRightHeadLightBroken,
				IsLeftHeadLightBroken = vehicle.IsLeftHeadLightBroken,
				IsRearBumperBrokenOff = vehicle.IsRearBumperBrokenOff,
				IsFrontBumperBrokenOff = vehicle.IsFrontBumperBrokenOff,
				IsTaxiLightOn = vehicle.IsTaxiLightOn,
				IsSearchLightOn = vehicle.IsSearchLightOn,
				IsInteriorLightOn = vehicle.IsInteriorLightOn,
				IsLightsOn = vehicle.AreLightsOn,
				IsHighBeamsOn = vehicle.AreHighBeamsOn,
				IsEngineRunning = vehicle.IsEngineRunning,
				IsDriveable = vehicle.IsDriveable,
				IsAlarmed = vehicle.IsAlarmSet,
				IsAlarmSounding = vehicle.IsAlarmSounding,
				LicensePlate = vehicle.Mods.LicensePlate,
				//Extras = vehicleExtras
			};
		}
	}
}
