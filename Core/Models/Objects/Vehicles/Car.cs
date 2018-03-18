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
                Handle = vehicle.Handle,
                Position = vehicle.Position,
            };

        }
    }

    
}