using IgiCore.Server.Models.Player;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server.Extentions
{
    public static class CitizenExtentions
    {
        public static Character ToLastCharacter(this Citizen citizen) => Character.GetLatestOrCreate(User.GetOrCreate(citizen));
    }
}
