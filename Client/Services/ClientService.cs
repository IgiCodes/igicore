using System.Threading.Tasks;

namespace IgiCore.Client.Services
{
	public abstract class ClientService : IClientService
	{
		public abstract Task Tick();
	}
}
