﻿using System;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using Newtonsoft.Json;

namespace IgiCore.Core.Models.Economy.Banking
{
    public class BankAtm : IBankATM
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Deleted { get; set; }
        public string Name { get; set; }
        public uint Hash { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        [JsonIgnore]
        public Vector3 Position
        {
            get => new Vector3(this.PosX, this.PosY, this.PosZ);
            set
            {
                this.PosX = value.X;
                this.PosY = value.Y;
                this.PosZ = value.Z;
            }
        }

        public BankAtm()
        {
            this.Id = GuidGenerator.GenerateTimeBasedGuid();
            Created = DateTime.UtcNow;
        }
    }
}
