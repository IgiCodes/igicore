using System.Threading.Tasks;

namespace IgiCore.Client.Services
{
    public abstract class Service : IService
    {
        public abstract Task OnTick(Client client);
    }
}