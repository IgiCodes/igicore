using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;

namespace IgiCore.Client.Services.World
{
	public class WeatherService : ClientService
	{
		protected static readonly string[] WeatherNames = {
			"EXTRASUNNY",
			"CLEAR",
			"CLOUDS",
			"SMOG",
			"FOGGY",
			"OVERCAST",
			"RAIN",
			"THUNDER",
			"CLEARING",
			"NEUTRAL",
			"SNOW",
			"BLIZZARD",
			"SNOWLIGHT",
			"XMAS"
		};

		public Weather Current { get; set; } = Weather.Christmas;
		public Weather Last { get; protected set; } = Weather.ExtraSunny;

		public override async Task Tick()
		{
			if (this.Last != this.Current)
			{
				this.Last = this.Current;

				Screen.ShowNotification("SetWeatherTypeOverTime");
				SetWeatherTypeOverTime(WeatherNames[(int)this.Last], 15);

				await BaseScript.Delay(15000);

				Screen.ShowNotification("Done");

				if (this.Current == Weather.ExtraSunny)
				{
					this.Current = Weather.Christmas;
				}
				else
				{
					this.Current = Weather.ExtraSunny;
				}
			}

			ClearOverrideWeather();
			ClearWeatherTypePersist();
			SetWeatherTypePersist(WeatherNames[(int)this.Last]);
			SetWeatherTypeNow(WeatherNames[(int)this.Last]);
			SetWeatherTypeNowPersist(WeatherNames[(int)this.Last]);

			SetForceVehicleTrails(this.Last == Weather.Christmas);
			SetForcePedFootstepsTracks(this.Last == Weather.Christmas);
		}
	}
}
