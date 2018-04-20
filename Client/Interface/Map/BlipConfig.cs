using CitizenFX.Core;

namespace IgiCore.Client.Interface.Map
{
	public class BlipConfig
	{
		public string Name { get; set; }
		public BlipSprite Sprite { get; set; }
		public float SpriteScale { get; set; } = 1;
		public Vector3 Position { get; set; }
		public float? Radius { get; set; }
		public bool PinMinimap { get; set; } = false;
		public BlipColor Color { get; set; } = BlipColor.White;
	}
}
