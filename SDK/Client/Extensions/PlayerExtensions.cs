using CitizenFX.Core;

namespace IgiCore.SDK.Client.Extensions
{
	public static class PlayerExtentions
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
	}
}
