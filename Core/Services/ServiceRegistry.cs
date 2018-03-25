using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;

namespace IgiCore.Core.Services
{
	public class ServiceRegistry : List<Service>
	{
		public void Initialise(EventHandlerDictionary handlers)
		{
			foreach (Service service in this)
			{
				service.Initialise();

				foreach (var e in service.Events)
				{
					handlers[e.Key] += e.Value;
				}
			}
		}

		public T First<T>() where T : class
		{
			return this.First(s => s is T) as T;
		}

		public IEnumerable<T> Where<T>() where T : class
		{
			return this.Where(s => s is T).Cast<T>();
		}
	}
}
