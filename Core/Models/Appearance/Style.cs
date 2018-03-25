using System;
using System.ComponentModel.DataAnnotations;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Appearance
{
	public class Style
	{
		[Key] public Guid Id { get; set; }
		public Component Face { get; set; }
		public Component Head { get; set; }
		public Component Hair { get; set; }
		public Component Torso { get; set; }
		public Component Torso2 { get; set; }
		public Component Legs { get; set; }
		public Component Hands { get; set; }
		public Component Shoes { get; set; }
		public Component Special1 { get; set; }
		public Component Special2 { get; set; }
		public Component Special3 { get; set; }
		public Component Textures { get; set; }

		public Prop Hat { get; set; }
		public Prop Glasses { get; set; }
		public Prop EarPiece { get; set; }
		public Prop Unknown3 { get; set; }
		public Prop Unknown4 { get; set; }
		public Prop Unknown5 { get; set; }
		public Prop Watch { get; set; }
		public Prop Wristband { get; set; }
		public Prop Unknown8 { get; set; }
		public Prop Unknown9 { get; set; }

		public Style()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();

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
