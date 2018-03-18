using System.Threading.Tasks;

namespace IgiCore.Client.Services
{
    public interface IService
    {
        Task Tick(Client client);
    }
}
