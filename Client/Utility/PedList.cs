using System.Collections;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace IgiCore.Client.Utility
{
	/// <summary>
	/// Gets all known world peds.
	/// </summary>
	/// <seealso cref="IEnumerable{Ped}" />
	/// <inheritdoc />
	public class PedList : IEnumerable<Ped>
	{
		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// An enumerator that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<Ped> GetEnumerator()
		{
			OutputArgument output = new OutputArgument();

			int handle = Function.Call<int>((Hash)(uint)Game.GenerateHash("FIND_FIRST_PED"), output);

			yield return new Ped(output.GetResult<int>());

			while (Function.Call<bool>((Hash)(uint)Game.GenerateHash("FIND_NEXT_PED"), handle, output))
			{
				yield return new Ped(output.GetResult<int>());
			}

			Function.Call((Hash)(uint)Game.GenerateHash("END_FIND_PED"), handle);
		}
	}
}
