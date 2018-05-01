using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace IgiCore.Client.Extensions
{
	public static class PlayerExtensions
	{
		/// <summary>
		/// Freezes the specified player in place.
		/// </summary>
		/// <param name="player">The player to freeze.</param>
		/// <param name="freeze">If set to <c>true</c> freeze the player, otherwise unfreeze.</param>
		public static void Freeze(this Player player, bool freeze = true)
		{
			player.CanControlCharacter = !freeze;
			player.Character.IsVisible = !freeze;
			player.Character.IsCollisionEnabled = !freeze;
			player.Character.IsPositionFrozen = freeze;
			player.Character.IsInvincible = freeze;
			player.Character.Task.ClearAllImmediately();
		}

		/// <summary>
		/// Unfreezes the specified player.
		/// </summary>
		/// <see cref="Freeze"/>
		/// <param name="player">The player to unfreeze.</param>
		public static void Unfreeze(this Player player) => player.Freeze(false);

		/// <summary>
		/// Spawns the player at the specified position.
		/// </summary>
		/// <param name="player">The player to spawn.</param>
		/// <param name="position">The position to spawn at.</param>
		/// <returns></returns>
		public static async Task Spawn(this Player player, Vector3 position)
		{
			player.Freeze();

			// Load map
			LoadScene(position.X, position.Y, position.Z);
			RequestCollisionAtCoord(position.X, position.Y, position.Z);

			// Swap model
			while (!await player.ChangeModel(new Model(PedHash.FreemodeMale01))) await BaseScript.Delay(10);

			// Not naked
			player.Character.Style.SetDefaultClothes();

			player.Character.Position = position;
			player.Character.ClearBloodDamage();
			player.Character.Weapons.Drop();
			player.WantedLevel = 0;
			
			player.Unfreeze();
		}
	}
}
