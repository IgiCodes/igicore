using System;
using System.Threading.Tasks;

namespace IgiCore.SDK.Client.Events
{
	public interface ITickManager
	{
		void Attach(Func<Task> callback);

		void Detach(Func<Task> callback);
	}
}
