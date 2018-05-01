using System;
using IgiCore.Core.Models.Player;

namespace IgiCore.Client.Models
{
    public class User : IUser
    {
        public Character Character { get; set; }
        public Guid Id { get; set; }
        public string SteamId { get; set; }
        public string Name { get; set; }
	    public DateTime? AcceptedRules { get; set; }
	    public DateTime LastConnected { get; set; }
	    public string LastIpAddress { get; set; }
	    public DateTime Created { get; set; }
    }
}
