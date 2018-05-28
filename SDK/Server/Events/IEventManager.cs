using System;
using System.Threading.Tasks;

namespace IgiCore.SDK.Server.Events
{
	public interface IEventManager
	{
		void On<T>(string @event, Action<T> action);

		void Raise<T>(string @event, T message);

		Task RaiseAsync<T>(string @event, T message);
	}
}
