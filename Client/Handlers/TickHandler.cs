using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IgiCore.Client.Handlers
{
	public static class TickHandler
	{
		private static readonly Dictionary<Type, Func<Task>> Handlers = new Dictionary<Type, Func<Task>>();

		/// <summary>
		/// Attaches the specified handler to the global tick event.
		/// </summary>
		/// <typeparam name="T">Type to use as handler key.</typeparam>
		/// <param name="task">The tick handler task.</param>
		public static void Attach<T>(Func<Task> task)
		{
			Handlers.Add(typeof(T), task);

			Client.Instance.AttachTickHandler(task);
		}

		/// <summary>
		/// Dettaches the specified handler from the global tick event.
		/// </summary>
		/// <typeparam name="T">Type to use as handler key.</typeparam>
		public static void Dettach<T>()
		{
			if (!Handlers.ContainsKey(typeof(T))) return;

			Client.Instance.DettachTickHandler(Handlers[typeof(T)]);
		}
	}
}
