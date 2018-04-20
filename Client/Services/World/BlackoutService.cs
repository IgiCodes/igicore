using System.Threading.Tasks;

namespace IgiCore.Client.Services.World
{
	public class BlackoutService : ClientService
	{
		public bool Enabled { get; set; } = false;

		public override async Task Tick()
		{
			CitizenFX.Core.World.Blackout = this.Enabled;
		}
	}
}
