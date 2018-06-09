using System;
using System.Threading.Tasks;

namespace IgiCore.SDK.Server.Events
{
	public interface IEventManager
	{
		void On(string @event, Action action);

		void On<T>(string @event, Action<T> action);

		void On<T1, T2>(string @event, Action<T1, T2> action);

		void On<T1, T2, T3>(string @event, Action<T1, T2, T3> action);

		void On<T1, T2, T3, T4>(string @event, Action<T1, T2, T3, T4> action);

		void On<T1, T2, T3, T4, T5>(string @event, Action<T1, T2, T3, T4, T5> action);

		void Raise(string @event);

		void Raise<T>(string @event, T p1);

		void Raise<T1, T2>(string @event, T1 p1, T2 p2);

		void Raise<T1, T2, T3>(string @event, T1 p1, T2 p2, T3 p3);

		void Raise<T1, T2, T3, T4>(string @event, T1 p1, T2 p2, T3 p3, T4 p4);

		void Raise<T1, T2, T3, T4, T5>(string @event, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);


		Task RaiseAsync(string @event);

		Task RaiseAsync<T>(string @event, T p1);

		Task RaiseAsync<T1, T2>(string @event, T1 p1, T2 p2);

		Task RaiseAsync<T1, T2, T3>(string @event, T1 p1, T2 p2, T3 p3);

		Task RaiseAsync<T1, T2, T3, T4>(string @event, T1 p1, T2 p2, T3 p3, T4 p4);

		Task RaiseAsync<T1, T2, T3, T4, T5>(string @event, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);
	}
}
