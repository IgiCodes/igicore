using System.Collections.Generic;
using System.Linq;

namespace IgiCore.Core.Services
{
    public class ServiceRegistry : List<Service>
    {
        public void Initialize()
        {
            foreach (Service service in this) service.Initialize();
		}

        public T First<T>() where T : class { return this.First(s => s is T) as T; }

        public IEnumerable<T> Where<T>() where T : class { return this.Where(s => s is T).Cast<T>(); }
    }
}
