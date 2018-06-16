namespace Roleplay.Vehicles.Core.Models
{
    public interface ICar : IRoadVehicle
    {
        Trailer Trailer { get; set; }
        Vehicle TowedVehicle { get; set; }
    }
}
