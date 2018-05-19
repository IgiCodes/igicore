using System.Collections.Generic;
using IgiCore.Client.Models.World;

namespace IgiCore.Client.Managers.World
{
    public class NpcManager : Manager
    {
        public List<Npc> Npcs { get; set; }

        public override void Dispose()
        {

        }
    }
}
