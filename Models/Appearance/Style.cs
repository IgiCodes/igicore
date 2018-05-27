using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace IgiCore.Models.Appearance
{
	[PublicAPI]
	public class Style
	{
		[Key]
		[Required]
		public Guid Id { get; set; }

		[Required]
		public Component Face { get; set; }

		[Required]
		public Component Head { get; set; }

		[Required]
		public Component Hair { get; set; }

		[Required]
		public Component Torso { get; set; }

		[Required]
		public Component Torso2 { get; set; }

		[Required]
		public Component Legs { get; set; }

		[Required]
		public Component Hands { get; set; }

		[Required]
		public Component Shoes { get; set; }

		[Required]
		public Component Special1 { get; set; }

		[Required]
		public Component Special2 { get; set; }

		[Required]
		public Component Special3 { get; set; }

		[Required]
		public Component Textures { get; set; }

		[Required]
		public Prop Hat { get; set; }

		[Required]
		public Prop Glasses { get; set; }

		[Required]
		public Prop EarPiece { get; set; }

		[Required]
		public Prop Unknown3 { get; set; }

		[Required]
		public Prop Unknown4 { get; set; }

		[Required]
		public Prop Unknown5 { get; set; }

		[Required]
		public Prop Watch { get; set; }

		[Required]
		public Prop Wristband { get; set; }

		[Required]
		public Prop Unknown8 { get; set; }

		[Required]
		public Prop Unknown9 { get; set; }

		public Style()
		{
			this.Face = new Component();
			this.Head = new Component();
			this.Hair = new Component();
			this.Torso = new Component();
			this.Torso2 = new Component();
			this.Legs = new Component();
			this.Hands = new Component();
			this.Shoes = new Component();
			this.Special1 = new Component();
			this.Special2 = new Component();
			this.Special3 = new Component();
			this.Textures = new Component();

			this.Hat = new Prop();
			this.Glasses = new Prop();
			this.EarPiece = new Prop();
			this.Unknown3 = new Prop();
			this.Unknown4 = new Prop();
			this.Unknown5 = new Prop();
			this.Watch = new Prop();
			this.Wristband = new Prop();
			this.Unknown8 = new Prop();
			this.Unknown9 = new Prop();
		}
	}
}
