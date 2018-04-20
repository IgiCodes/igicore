using System;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace IgiCore.Client.Interface
{
	public static class UI
	{
		public static void DrawSprite(string pinnedDict, string pinnedName, PointF position, SizeF size, SizeF offset, Color color, bool centered)
		{
			float screenWidth = Screen.Width;
			float screenHeight = Screen.Height;
			float scaleX = size.Width / screenWidth;
			float scaleY = size.Height / screenHeight;
			float positionX = (position.X + offset.Width) / screenWidth;
			float positionY = (position.Y + offset.Height) / screenHeight;

			if (!centered)
			{
				positionX += scaleX * 0.5f;
				positionY += scaleY * 0.5f;
			}

			if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, pinnedDict)) Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, pinnedDict, true);

			Function.Call(Hash.DRAW_SPRITE, pinnedDict, pinnedName, positionX, positionY, scaleX, scaleY, 0, color.R, color.G, color.B, color.A);
		}

		public static PointF MousePosition()
		{
			int mouseX = (int)Math.Round(Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorX) * Screen.Width);
			int mouseY = (int)Math.Round(Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorY) * Screen.Height);

			return new PointF(mouseX, mouseY);
		}

		public static void ShowNotification(string message)
		{
			Screen.ShowNotification(message);
		}

		public static async Task FadeScreenOut(int msec = 0)
		{
			Client.Log("FadeScreenOut START");
			Screen.Fading.FadeOut(msec);
			while (Screen.Fading.IsFadingOut) await BaseScript.Delay(1);
			Client.Log("FadeScreenOut END");
		}

		public static async Task FadeScreenIn(int msec = 0)
		{
			Client.Log("FadeScreenIn START");
			Screen.Fading.FadeIn(msec);
			while (Screen.Fading.IsFadingIn) await BaseScript.Delay(1);
			Client.Log("FadeScreenIn END");
		}

		public static void ShutdownLoadingScreen()
		{
			API.ShutdownLoadingScreen();
		}
	}
}
