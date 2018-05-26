using System;
using IgiCore.SDK.Core;

namespace IgiCore.SDK.Server.Rpc
{
	public interface IClientEvent
	{
		//void On(Action<Player> callback);
		//void On(Action<Player, string, CallbackDelegate> callback);
		void On(Action<Client> callback);
		void On<T>(Action<Client, T> callback);
		void On<T1, T2>(Action<Client, T1, T2> callback);
		void On<T1, T2, T3>(Action<Client, T1, T2, T3> callback);
		void On<T1, T2, T3, T4>(Action<Client, T1, T2, T3, T4> callback);
		void On<T1, T2, T3, T4, T5>(Action<Client, T1, T2, T3, T4, T5> callback);
	}
}
