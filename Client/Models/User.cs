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
    }
}
