using System;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using Newtonsoft.Json;

namespace IgiCore.Core.Models.Economy.Banking
{
	public class BankBranch : IBankBranch
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Deleted { get; set; }
		public string Name { get; set; }
	    public float PosX { get; set; }
	    public float PosY { get; set; }
	    public float PosZ { get; set; }
        public float Heading { get; set; }

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

        public BankBranch()
	    {
	        this.Id = GuidGenerator.GenerateTimeBasedGuid();
            this.Created = DateTime.UtcNow;
	    }
    }
}
