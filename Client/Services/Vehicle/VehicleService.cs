using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.Client.Controllers.Player;
using IgiCore.Client.Extensions;
using IgiCore.Client.Input;
using IgiCore.Client.Rpc;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using Newtonsoft.Json;

namespace IgiCore.Client.Services.Vehicle
{
	public class VehicleService : ClientService
	{
		private const int VehicleLoadDistance = 500;

		private float dmgAmount = 0f;
		private float dmgRadius = 0f;
		private Vector3 deformOffset = new Vector3();

		public List<TrackedVehicle> Tracked { get; set; } = new List<TrackedVehicle>();

		public override async Task Tick()
		{
			//CitizenFX.Core.Vehicle vehicle = Game.PlayerPed.CurrentVehicle;



			//if (Game.PlayerPed.IsInVehicle())
			//{
			//	Vector3 vehicleDimensions = vehicle.Model.GetDimensions();
			//	Vector3 frontOffset = new Vector3(0f, vehicleDimensions.Y / 2, 0f);
			//	Vector3 rearOffset = new Vector3(0f, (vehicleDimensions.Y / 2) * -1, 0f);
			//	Vector3 rightOffset = new Vector3(vehicleDimensions.X / 2, 0f, 0f);
			//	Vector3 leftOffset = new Vector3((vehicleDimensions.X / 2) * -1, 0f, 0f);
			//	Vector3 topOffset = new Vector3(0f, 0f, vehicleDimensions.Z / 2);
			//	Vector3 bottomOffser = new Vector3(0f, 0f, (vehicleDimensions.Z / 2) * -1);

			//	CitizenFX.Core.World.DrawMarker(MarkerType.DebugSphere, vehicle.GetOffsetPosition(this.deformOffset), Vector3.Zero, Vector3.Zero, new Vector3((float)(this.dmgRadius * 0.05)), Color.FromArgb(40, 255, 0, 0));
			//	Vector3 deformation = API.GetVehicleDeformationAtPos(vehicle.Handle, deformOffset.X, deformOffset.Y, deformOffset.Z);
			//	int count = 0;
			//	this.bones.ForEach((bone) =>
			//	{
			//		if (vehicle.Bones.HasBone(bone)) count++;
			//	});
			//	new Text($"Bones: {count} | Deformation: {deformation} | Offset: {this.deformOffset} | DmgAmount: {dmgAmount} | DmgRadius: {dmgRadius} | Dimensions: {vehicleDimensions}", new PointF(50, 100), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();


			//	if (Input.Input.IsControlJustPressed(Control.InteractionMenu))
			//	{

			//		Vector3 bonePos = vehicle.Bones["door_pside_f"].Position;
			//		Vector3 boneOffset = vehicle.GetPositionOffset(bonePos);
			//		Vector3 boneDeformation = API.GetVehicleDeformationAtPos(vehicle.Handle, deformOffset.X, deformOffset.Y, deformOffset.Z);
			//		Client.Log($"Deformation Before: {boneDeformation}");
			//		API.SetVehicleDeformationFixed(vehicle.Handle);
			//		vehicle.Repair();
			//		API.ApplyForceToEntity(vehicle.Handle, 1, boneDeformation.X, boneDeformation.Y, boneDeformation.Z, bonePos.X, bonePos.Y, bonePos.Z, vehicle.Bones["door_pside_f"].Index, true, true, true, true, true);
			//		//Function.Call(Hash.SET_VEHICLE_DAMAGE, vehicle.Handle, deformOffset.X, deformOffset.Y, deformOffset.Z, this.dmgAmount, this.dmgRadius, true);
			//	}

			//	if (Input.Input.IsControlJustPressed(Control.InteractionMenu, modifier: InputModifier.Ctrl))
			//	{
			//		this.bones.ForEach((bone) =>
			//		{
			//			if (vehicle.Bones.HasBone(bone)) Client.Log(bone);
			//		});
			//		Client.Log($"Deformation Length: {deformation.Length()}");
			//		Client.Log($"Deformation Length sqrd: {deformation.LengthSquared()}");
			//		Client.Log($"Deformation Offset plus: {deformOffset + deformation}");
			//		Client.Log($"Deformation Offset minus: {deformOffset - deformation}");
			//	}

			//	if (Input.Input.IsControlJustPressed(Control.Jump) && Game.PlayerPed.IsInVehicle())
			//	{
			//		API.SetVehicleDeformationFixed(vehicle.Handle);
			//		vehicle.Repair();
			//	}
			//}

			//if (Input.Input.IsControlJustPressed(Control.PhoneUp)) this.dmgAmount += 100f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneUp, modifier: InputModifier.Shift)) this.dmgAmount += 1000f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneDown)) this.dmgAmount -= 100f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneUp, modifier: InputModifier.Shift)) this.dmgAmount -= 1000f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneRight)) this.dmgRadius += 0.1f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneRight, modifier: InputModifier.Shift)) this.dmgRadius += 1f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneLeft)) this.dmgRadius -= 0.1f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneLeft, modifier: InputModifier.Shift)) this.dmgRadius -= 1f;

			//if (Input.Input.IsControlJustPressed(Control.PhoneUp, modifier: InputModifier.Ctrl)) this.deformOffset.X += 0.1f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneDown, modifier: InputModifier.Ctrl)) this.deformOffset.X -= 0.1f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneUp, modifier: InputModifier.Alt)) this.deformOffset.Y += 0.1f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneDown, modifier: InputModifier.Alt)) this.deformOffset.Y -= 0.1f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneUp, modifier: InputModifier.Alt | InputModifier.Ctrl)) this.deformOffset.Z += 0.1f;
			//if (Input.Input.IsControlJustPressed(Control.PhoneDown, modifier: InputModifier.Alt | InputModifier.Ctrl)) this.deformOffset.Z -= 0.1f;

			//if (Input.Input.IsControlJustPressed(Control.PhoneUp) && Game.PlayerPed.IsInVehicle())
			//{
			//	TaskSequence ts = new TaskSequence();
			//	ts.AddTask.PlayAnimation("veh@std@ds@base", API.GetFollowVehicleCamViewMode() == 4 ? "pov_start_engine" : "start_engine");
			//	ts.Close();
			//	Game.PlayerPed.Task.PerformSequence(ts);
			//	vehicle.ToggleEngine(!vehicle.IsEngineRunning, false, true);
			//	//vehicle.IsEngineRunning = !vehicle.IsEngineRunning;
			//}




			await Update();
			await Save();

			await BaseScript.Delay(1000);


		}

		private async Task Update()
		{
			foreach (TrackedVehicle trackedVehicle in this.Tracked.ToList())
			{
				int vehicleHandle = API.NetToVeh(trackedVehicle.NetId);
				var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
				var closestPlayer = new CitizenFX.Core.Player(API.GetNearestPlayerToEntity(citVeh.Handle));

				if (closestPlayer == Game.Player || !API.NetworkIsPlayerConnected(closestPlayer.Handle))
				{
					if (!(Vector3.Distance(Game.Player.Character.Position, citVeh.Position) > VehicleLoadDistance)) continue;

					citVeh.Delete();
					this.Tracked.Remove(trackedVehicle);
					Server.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:unclaim")
						.Attach(trackedVehicle.NetId)
						.Trigger();
				}
				else
				{
					int netId = API.NetworkGetNetworkIdFromEntity(citVeh.Handle);

					Car car = (Car)citVeh;
					car.NetId = netId;

					Client.Log($"Transfering vehicle to player: {closestPlayer.ServerId}  -  {car.Handle}");
					Server.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:transfer")
						.Attach(car)
						.Attach(closestPlayer.ServerId)
						.Trigger();
				}
			}
		}

		private async Task Save()
		{
			foreach (TrackedVehicle trackedVehicle in this.Tracked)
			{
				int vehicleHandle = API.NetToVeh(trackedVehicle.NetId);
				var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
				int netId = API.NetworkGetNetworkIdFromEntity(citVeh.Handle);

				Core.Models.Objects.Vehicles.Vehicle vehicle = await citVeh.ToVehicle(trackedVehicle.Id);

				vehicle.TrackingUserId = Client.Instance.Controllers.First<UserController>().User.Id;
				vehicle.NetId = netId;
				vehicle.Hash = citVeh.Model.Hash;

				switch (trackedVehicle.Type.VehicleType().Name)
				{
					case "Car":
						//Car car = (Car)vehicle;
						// Add car specific props...
						Server.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:save")
							.Attach(vehicle)
							.Trigger();
						break;

					default:
						Server.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:save")
							.Attach(vehicle)
							.Trigger();
						break;
				}
			}
		}

		public class TrackedVehicle
		{
			public Guid Id { get; set; }
			public int NetId { get; set; }
			public Type Type { get; set; }
		}
	}
}
