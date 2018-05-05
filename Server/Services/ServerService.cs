using System.Threading.Tasks;
using IgiCore.Core.Services;
using IgiCore.Server.Models.Player;

namespace IgiCore.Server.Services
{
    public abstract class ServerService : Service, IServerService
    {
        public virtual async Task<Character> OnCharacterCreate(Character character) => character;
    }
}
