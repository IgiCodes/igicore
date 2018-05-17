using System.Collections.Generic;
using System.Linq;
using IgiCore.Core.Utility;

namespace IgiCore.Client.Services
{
	public class ServiceRegistry : Registry<ClientService>
	{
		public void Initialize()
		{
			foreach (var service in this) Client.Instance.AttachTickHandler(service.Tick);
		}
	}
}
