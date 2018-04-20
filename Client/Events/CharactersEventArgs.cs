using System;
using System.Collections.Generic;
using IgiCore.Client.Models;

namespace IgiCore.Client.Events
{
	public class CharactersEventArgs : EventArgs
	{
		public List<Character> Characters { get; }

		public CharactersEventArgs(List<Character> characters)
		{
			this.Characters = characters;
		}
	}
}
