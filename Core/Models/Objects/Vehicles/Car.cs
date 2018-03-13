namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class Car : Vehicle, ICar
    {
        public Trailer Trailer { get; set; }
        public Vehicle TowedVehicle { get; set; }
    }
}