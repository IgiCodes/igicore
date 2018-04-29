using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace IgiCore.Client.Services.World
{
	public class DateTimeService : ClientService
	{
		public override async Task Tick()
		{
            //CitizenFX.Core.World.CurrentDate = DateTime.UtcNow; // TODO: Timezone - TimeZoneInfo won't work in Mono...
            DateTime time = new DateTime(2018, 1, 1, 12, 0, 0);
		    CitizenFX.Core.World.CurrentDate = time;
            //NetworkOverrideClockTime(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second); // This correctly sets the time but seems to break the Esc menu clock/money/avatar...
		    NetworkOverrideClockTime(time.Hour, time.Minute, time.Second); // This correctly sets the time but seems to break the Esc menu clock/money/avatar...
        }
	}
}
