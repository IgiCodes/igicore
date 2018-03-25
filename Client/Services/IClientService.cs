using System.Threading.Tasks;

namespace IgiCore.Client.Services
{
	public interface IClientService
	{
		Task Tick(Client client);
	}
}
