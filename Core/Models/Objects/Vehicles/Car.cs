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
