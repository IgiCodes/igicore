using System;
using IgiCore.Client.Models;

namespace IgiCore.Client.Events
{
	public class CharacterEventArgs : EventArgs
	{
		public Character Character { get; }

		public CharacterEventArgs(Character character)
		{
			this.Character = character;
		}
	}
}
