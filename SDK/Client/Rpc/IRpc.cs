using System;
using System.Threading.Tasks;

namespace IgiCore.SDK.Client.Rpc
{
	public interface IRpc
	{
		void Trigger(params object[] payloads);

		void On(Action<IRpcEvent> callback);

		void On<T>(Action<IRpcEvent, T> callback);

		void On<T1, T2>(Action<IRpcEvent, T1, T2> callback);

		void On<T1, T2, T3>(Action<IRpcEvent, T1, T2, T3> callback);

		void On<T1, T2, T3, T4>(Action<IRpcEvent, T1, T2, T3, T4> callback);

		void On<T1, T2, T3, T4, T5>(Action<IRpcEvent, T1, T2, T3, T4, T5> callback);

		Task Request(params object[] payloads);

		Task<T> Request<T>(params object[] payloads);

		Task<Tuple<T1, T2>> Request<T1, T2>(params object[] payloads);

		Task<Tuple<T1, T2, T3>> Request<T1, T2, T3>(params object[] payloads);

		Task<Tuple<T1, T2, T3, T4>> Request<T1, T2, T3, T4>(params object[] payloads);

		Task<Tuple<T1, T2, T3, T4, T5>> Request<T1, T2, T3, T4, T5>(params object[] payloads);
	}
}
