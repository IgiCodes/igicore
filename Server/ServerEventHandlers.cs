using CitizenFX.Core;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Server.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using IgiCore.Core.Extensions;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server
{
    public partial class Server
    {
        private void OnPlayerConnecting([FromSource] Citizen citizen, string playerName, CallbackDelegate kickReason)
        {
            User.GetOrCreate(citizen);
        }

        private void OnPlayerDropped([FromSource] Citizen citizen, string disconnectMessage, CallbackDelegate kickReason)
        {
            //Debug.WriteLine($"Disconnected: {citizen.Name}");
        }

        private void OnChatMessage(int playerId, string playerName, string message)
        {
            Citizen citizen = Players[playerId];

            List<string> args = message.Split(' ').ToList();
            string command = args.First().ToLowerInvariant();
            args = args.Skip(1).ToList();

            switch (command)
            {
                case "/newchar":
                    Log("/newchar command called");

                    TriggerClientEvent(citizen, "igi:character:new", JsonConvert.SerializeObject(NewCharCommand(citizen, args[0])));

                    break;
                case "/char":
                    Log("/char command called");

                    TriggerClientEvent(citizen, "igi:character:load", JsonConvert.SerializeObject(GetCharCommand(citizen, args[0])));

                    break;
                case "/gps":
                    Log("/gps command called");

                    TriggerClientEvent(citizen, "igi:user:gps");

                    break;
                case "/component":
                    Log("/component command called");

                    TriggerClientEvent(citizen, "igi:character:component:set", int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]));

                    break;
                case "/prop":
                    Log("/prop command called");

                    TriggerClientEvent(citizen, "igi:character:prop:set", int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]));

                    break;
                case "/car":
                    Log("/car command called");

                    var car = new Car
                    {
                        Id = GuidGenerator.GenerateTimeBasedGuid(),
                        Hash = (uint)VehicleHash.Elegy,
                        Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f },
                        Seats = new List<VehicleSeat>
                        {
                            new VehicleSeat
                            {
                                Index = VehicleSeatIndex.LeftFront
                            },
                            new VehicleSeat
                            {
                                Index = VehicleSeatIndex.RightFront
                            },
                            new VehicleSeat
                            {
                                Index = VehicleSeatIndex.LeftRear
                            },
                            new VehicleSeat
                            {
                                Index = VehicleSeatIndex.RightRear
                            }
                        },
                        Wheels = new List<VehicleWheel>
                        {
                            new VehicleWheel
                            {
                                Index = 0,
                                IsBurst = false,
                                Type = VehicleWheelType.Sport
                            },
                            new VehicleWheel
                            {
                                Index = 0,
                                IsBurst = false,
                                Type = VehicleWheelType.Sport
                            },
                            new VehicleWheel
                            {
                                Index = 0,
                                IsBurst = false,
                                Type = VehicleWheelType.Sport
                            },
                            new VehicleWheel
                            {
                                Index = 0,
                                IsBurst = false,
                                Type = VehicleWheelType.Sport
                            }
                        },
                        Windows = new List<VehicleWindow>
                        {
                            new VehicleWindow
                            {
                                Index = VehicleWindowIndex.FrontLeftWindow,
                                IsIntact = false,
                                IsRolledDown = false
                            },
                            new VehicleWindow
                            {
                                Index = VehicleWindowIndex.FrontRightWindow,
                                IsIntact = false,
                                IsRolledDown = false
                            },
                            new VehicleWindow
                            {
                                Index = VehicleWindowIndex.BackLeftWindow,
                                IsIntact = false,
                                IsRolledDown = false
                            },
                            new VehicleWindow
                            {
                                Index = VehicleWindowIndex.BackRightWindow,
                                IsIntact = false,
                                IsRolledDown = false
                            }
                        },
                        Doors = new List<VehicleDoor>
                        {
                            new VehicleDoor()
                            {
                                Index = VehicleDoorIndex.FrontLeftDoor,
                            },
                            new VehicleDoor()
                            {
                                Index = VehicleDoorIndex.FrontRightDoor,
                            },
                            new VehicleDoor()
                            {
                                Index = VehicleDoorIndex.BackLeftDoor,
                            },
                            new VehicleDoor()
                            {
                                Index = VehicleDoorIndex.BackRightDoor,
                            },
                            new VehicleDoor()
                            {
                                Index = VehicleDoorIndex.Hood,
                            },
                            new VehicleDoor()
                            {
                                Index = VehicleDoorIndex.Trunk,
                            },
                        }
                    };

                    
                    Db.Cars.Add(car);
                    Db.SaveChanges();

                    Log($"Sending {car.Id}");

                    TriggerClientEvent(citizen, "igi:car:spawn", JsonConvert.SerializeObject(car));
                    
                    break;
                default:
                    Log("Unknown command");

                    break;
            }
        }
    }
}
