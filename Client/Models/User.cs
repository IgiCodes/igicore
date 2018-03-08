using CitizenFX.Core;
using System.Collections.Generic;
using IgiCore.Core.Models;
using System;

namespace IgiCore.Client.Models
{
    public class User : IUser
    { 
        public Guid Id { get; set; }
        public string SteamId { get; set; }
        public string Name { get; set; }
        List<Character> Characters { get; set; }
    }
}
