using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.SDK.Core.Rpc;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Commands
{
	public class BikeCommand : Command
	{
		public override string Name => "bike";

		public override async Task RunCommand(Player player, List<string> args)
		{
			Bike bike = new Bike
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Hash = (uint)VehicleHash.Double,
				Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f }
			};

			Server.Db.Bikes.Add(bike);
			await Server.Db.SaveChangesAsync();

			player
				.Event(RpcEvents.BikeSpawn)
				.Attach(bike)
				.Trigger();
		}
	}
}
