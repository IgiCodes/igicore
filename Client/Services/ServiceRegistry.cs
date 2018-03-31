using System.Collections.Generic;
using System.Linq;

namespace IgiCore.Client.Services
{
    public class ServiceRegistry : List<ClientService>
    {
        public T First<T>() where T : class { return this.First(s => s is T) as T; }

        public IEnumerable<T> Where<T>() where T : class { return this.Where(s => s is T).Cast<T>(); }
    }
}
