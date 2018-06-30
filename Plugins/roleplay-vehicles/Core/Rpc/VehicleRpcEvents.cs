namespace Roleplay.Vehicles.Core.Rpc
{
	public static class VehicleRpcEvents
	{
		public const string VehicleSave = "igi:vehicle:save";
		public const string SavePosition = "igi:vehicle:save:position";
		public const string SaveHeading = "igi:vehicle:save:heading";
		public const string SaveLockStatus = "igi:vehicle:save:lockstatus";
		public const string SaveBodyHealth = "igi:vehicle:save:bodyhealth";
		public const string SaveEngineHealth = "igi:vehicle:save:enginehealth";
		public const string SavePetrolTankHealth = "igi:vehicle:save:petroltankhealth";
		public const string SaveDirtLevel = "igi:vehicle:save:dirtlevel";
		public const string SaveFuelLevel = "igi:vehicle:save:fuellevel";
		public const string SaveOilLevel = "igi:vehicle:save:oillevel";

		public const string CarCreate = "igi:car:create";
		public const string CarSave = "igi:car:save";
		public const string CarTransfer = "igi:car:transfer";
		public const string CarClaim = "igi:car:claim";
		public const string CarUnclaim = "igi:car:unclaim";

		public const string BikeCreate = "igi:bike:create";
		public const string BikeSave = "igi:bike:save";
		public const string BikeTransfer = "igi:bike:transfer";
		public const string BikeClaim = "igi:bike:claim";
		public const string BikeUnclaim = "igi:bike:unclaim";
	}
}
