using System;
using IgiCore.Client.Models;

namespace IgiCore.Client.Events
{
	public class UserEventArgs : EventArgs
	{
		public User User { get; }

		public UserEventArgs(User user)
		{
			this.User = user;
		}
	}
}
