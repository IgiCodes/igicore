using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Commands
{
	public class BikeCommand : Command
	{
		public override string Name => "bike";

		public override async Task RunCommand(Player player, List<string> args)
		{
			Tuple<Vector3, float> charPos = await player.Event(RpcEvents.GetCharacterPosition).Request<Vector3, float>();

			Bike bike = new Bike
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Hash = (uint)VehicleHash.Double,
				Position = charPos.Item1.GetPositionInFrontOfPed(charPos.Item2, 10f),
				PrimaryColor = new VehicleColor
				{
					StockColor = VehicleStockColor.HotPink,
					CustomColor = new Color(),
					IsCustom = false
				},
				SecondaryColor = new VehicleColor
				{
					StockColor = VehicleStockColor.MattePurple,
					CustomColor = new Color(),
					IsCustom = false
				},
				PearescentColor = VehicleStockColor.HotPink
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
