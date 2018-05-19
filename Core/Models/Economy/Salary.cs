using System;
using CitizenFX.Core;
using IgiCore.Core.Models.Groups;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace IgiCore.Core.Models.Economy
{
    public class Salary : Model
    {
        public TimeSpan Frequency { get; set; }
        public double Amount { get; set; }
        [CanBeNull] public Position Position { get; set; }
        [CanBeNull] public float Radius { get; set; }
        public bool Active { get; set; }

		public virtual GroupMember Member { get; set; }
    }
}
