using System.Threading.Tasks;

namespace IgiCore.Core.Services
{
    public abstract class Service : IService
    {
        public abstract Task Initialize();
    }
}
