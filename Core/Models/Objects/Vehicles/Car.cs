using System;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class Car : Vehicle, ICar
    {
        public Trailer Trailer { get; set; }
        public Vehicle TowedVehicle { get; set; }

        public static implicit operator Car(CitizenFX.Core.Vehicle vehicle)
        {
            return new Car
            {
				Id = Guid.Empty,
                Handle = vehicle.Handle,
                Position = vehicle.Position,
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
