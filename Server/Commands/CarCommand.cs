using System;
using System.Collections.Generic;
using System.Linq;
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
	public class CarCommand : Command
	{
		public override string Name => "car";

		public override async Task RunCommand(Player player, List<string> args)
		{
			Tuple<Vector3, float> charPos = await player.Event(RpcEvents.GetCharacterPosition).Request<Vector3, float>();
			VehicleHash carName = VehicleHash.Elegy;
			var dict = Enum.GetValues(typeof(VehicleHash))
				.Cast<VehicleHash>()
				.ToDictionary(t => (uint)t, t => t.ToString().ToLower());
			if (args.FirstOrDefault() != null) carName = dict.Where(d => d.Value == args[0].ToLower()).Select(d => d.Key).Cast<VehicleHash>().FirstOrDefault();
			if (carName == 0) carName = VehicleHash.Elegy;
			Server.Log(carName.ToString());
			Car car = new Car
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Hash = (uint)carName,
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
				PearescentColor = VehicleStockColor.HotPink,
				Seats = new List<VehicleSeat>(),
				Wheels = new List<VehicleWheel>(),
				Windows = new List<VehicleWindow>(),
				Doors = new List<VehicleDoor>()
			};

			Server.Db.Cars.Add(car);
			await Server.Db.SaveChangesAsync();

			player
				.Event(RpcEvents.CarCreate)
				.Attach(car)
				.Trigger();
		}
	}
}
