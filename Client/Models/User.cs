using System;
using IgiCore.Core.Models;

namespace IgiCore.Client.Models
{
    public class User : IUser
    {
        public Guid Id { get; set; }
        public string SteamId { get; set; }
        public string Name { get; set; }

    }
}
