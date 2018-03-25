using System;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class Trailer : Vehicle, ITrailer
    {

        public static implicit operator Trailer(CitizenFX.Core.Vehicle vehicle)
        {
            
            
            return new Trailer
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