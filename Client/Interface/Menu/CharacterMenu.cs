using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CitizenFX.Core.UI;
using NativeUI;

namespace IgiCore.Client.Interface.Menu
{
	public class CharacterMenu : UIMenu
	{
		public readonly List<KeyValuePair<string, string>> WalkingStyles = new List<KeyValuePair<string, string>> {
			new KeyValuePair<string, string>("Reset to default", null),
			new KeyValuePair<string, string>("Alien", "move_m@alien"),
			//new KeyValuePair<string, string>("Bail Bond", "move_m@bail_bond"),
			new KeyValuePair<string, string>("Drunk (Slightly)", "move_m@drunk@slightlydrunk"),
			new KeyValuePair<string, string>("Drunk (Very)", "move_m@drunk@verydrunk"),
			new KeyValuePair<string, string>("Hobo", "move_m@hobo@a"),
			//new KeyValuePair<string, string>("Joy", "move_m@joy"),
			new KeyValuePair<string, string>("Money", "move_m@money"),
			//new KeyValuePair<string, string>("Non Chalant", "move_m@non_chalant"),
			new KeyValuePair<string, string>("Posh", "move_m@posh@"),
			//new KeyValuePair<string, string>("Powerwalk", "move_m@powerwalk"),
			new KeyValuePair<string, string>("Sad", "move_m@sad@a"),
			new KeyValuePair<string, string>("Shady", "move_m@shadyped@a"),
			new KeyValuePair<string, string>("Swagger", "move_m@swagger"),
			//new KeyValuePair<string, string>("Tired", "move_m@tired"),
			new KeyValuePair<string, string>("Tough Guy", "move_m@tough_guy@")
		};

		public UIMenuListItem WalkingStyleMenu { get; }

		public CharacterMenu() : base("Character", "~b~CHARACTER MENU", new PointF(0, 0), "commonmenu", "interaction_bgd")
		{
			this.WalkingStyleMenu = new UIMenuListItem("Walking Style", this.WalkingStyles.Select(v => v.Key).Cast<dynamic>().ToList(), this.WalkingStyles.FindIndex(s => s.Value == Client.Instance.User?.Character?.WalkingStyle), "Set your character's walking style");
			AddItem(this.WalkingStyleMenu);

			this.OnListChange += CharacterMenu_OnListChange;
		}

		private async void CharacterMenu_OnListChange(UIMenu sender, UIMenuListItem listItem, int newIndex)
		{
			if (listItem == this.WalkingStyleMenu)
			{
				var style = this.WalkingStyles[newIndex];

				//await Client.Instance.User.Character.SetWalkingStyle(style.Value);

				Screen.ShowNotification("~b~Walking style~w~ set to ~r~" + style.Key);
			}
		}
	}
}
