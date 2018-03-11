using System;
using IgiCore.Core.Models;

namespace IgiCore.Client.Models
{
    public class User : IUser
    {
        public Guid Id { get; set; }
        public string SteamId { get; set; }
        public string Name { get; set; }
        public Character Character { get; set; }


        public static void Load(Client client, User user)
        {
            client.User = user;
        }
    }
}
