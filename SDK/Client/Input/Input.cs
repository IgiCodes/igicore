using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;

namespace IgiCore.SDK.Client.Input
{
	public static class Input
	{
		public static Dictionary<InputModifier, int> ModifierFlagToKeyCode => new Dictionary<InputModifier, int>
		{
			[InputModifier.Ctrl] = 36,
			[InputModifier.Alt] = 19,
			[InputModifier.Shift] = 21
		};

		public static bool WasLastInputFromController => false; // !NativeWrappers.IsInputDisabled(2); TODO

		public static bool IsControlModifierPressed(InputModifier modifier)
		{
			if (modifier == InputModifier.Any) return true;

			InputModifier bitMask = 0;
			
			ModifierFlagToKeyCode.ToList().ForEach(w =>
			{
				if (Game.IsControlPressed(0, (Control)w.Value)) bitMask = bitMask | w.Key;
			});

			return bitMask == modifier;
		}

		public static bool IsAnyControlJustPressed()
		{
			return Enum.GetValues(typeof(Control)).Cast<Control>().Any(value => IsControlJustPressed(value));
		}

		public static bool IsControlJustPressed(Control control, bool keyboardOnly = true, InputModifier modifier = InputModifier.None)
		{
			return Game.IsControlJustPressed(0, control) && (!keyboardOnly || !WasLastInputFromController) && IsControlModifierPressed(modifier);
		}

		public static bool IsControlPressed(Control control, bool keyboardOnly = true, InputModifier modifier = InputModifier.None)
		{
			return Game.IsControlPressed(0, control) && (!keyboardOnly || !WasLastInputFromController) && IsControlModifierPressed(modifier);
		}

		public static bool IsControlJustReleased(Control control, bool keyboardOnly = true, InputModifier modifier = InputModifier.None)
		{
			return Game.IsControlJustReleased(0, control) && (!keyboardOnly || !WasLastInputFromController) && IsControlModifierPressed(modifier);
		}

		public static bool IsDisabledControlJustPressed(Control control, bool keyboardOnly = true, InputModifier modifier = InputModifier.None)
		{
			return Game.IsDisabledControlJustPressed(0, control) && (!keyboardOnly || !WasLastInputFromController) && IsControlModifierPressed(modifier);
		}

		public static bool IsDisabledControlJustReleased(Control control, bool keyboardOnly = true, InputModifier modifier = InputModifier.None)
		{
			return Game.IsDisabledControlJustReleased(0, control) && (!keyboardOnly || !WasLastInputFromController) && IsControlModifierPressed(modifier);
		}

		public static bool IsDisabledControlPressed(Control control, bool keyboardOnly = true, InputModifier modifier = InputModifier.None)
		{
			return Game.IsDisabledControlPressed(0, control) && (!keyboardOnly || !WasLastInputFromController) && IsControlModifierPressed(modifier);
		}

		public static bool IsEnabledControlJustPressed(Control control, bool keyboardOnly = true, InputModifier modifier = InputModifier.None)
		{
			return Game.IsEnabledControlJustPressed(0, control) && (!keyboardOnly || !WasLastInputFromController) && IsControlModifierPressed(modifier);
		}

		public static bool IsEnabledControlJustReleased(Control control, bool keyboardOnly = true, InputModifier modifier = InputModifier.None)
		{
			return Game.IsEnabledControlJustReleased(0, control) && (!keyboardOnly || !WasLastInputFromController) && IsControlModifierPressed(modifier);
		}

		public static bool IsEnabledControlPressed(Control control, bool keyboardOnly = true, InputModifier modifier = InputModifier.None)
		{
			return Game.IsEnabledControlPressed(0, control) && (!keyboardOnly || !WasLastInputFromController) && IsControlModifierPressed(modifier);
		}
	}
}
