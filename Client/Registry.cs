using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IgiCore.Client
{
	public abstract class Registry<TR> : Collection<TR>
	{
		public T First<T>() where T : class => this.First(s => s is T) as T;

		public IEnumerable<T> Where<T>() where T : class => this.Where(s => s is T).Cast<T>();
	}
}
