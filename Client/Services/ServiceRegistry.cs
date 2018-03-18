using System.Collections.Generic;
using System.Linq;

namespace IgiCore.Client.Services
{
    public class ServiceRegistry : List<Service>
    {
        public T First<T>() where T : class
        {
            return this.FirstOrDefault(s => s is T) as T;
        }
    }
}