namespace IgiCore.Core.Models.Objects.Vehicles
{
    public interface ICar : IRoadVehicle
    {
        Trailer Trailer { get; set; }
        Vehicle TowedVehicle { get; set; }
    }
}
