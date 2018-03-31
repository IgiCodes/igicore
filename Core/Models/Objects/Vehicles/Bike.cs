using System;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class Bike : Vehicle, IBike
    {
        public static implicit operator Bike(CitizenFX.Core.Vehicle vehicle)
        {
            return new Bike
            {
                Id = Guid.Empty,
                Hash = vehicle.Model.Hash,
                Handle = vehicle.Handle,
                Position = vehicle.Position,
                Heading = vehicle.Heading,
                BodyHealth = vehicle.BodyHealth,
                EngineHealth = vehicle.EngineHealth,
                DirtLevel = vehicle.DirtLevel,
                FuelLevel = vehicle.FuelLevel,
                OilLevel = vehicle.OilLevel,
                PetrolTankHealth = vehicle.PetrolTankHealth
            };
        }
    }
}
