using System.Threading.Tasks;

namespace IgiCore.Client.Services
{
    public interface IService
    {
        Task OnTick(Client client);
    }
}
