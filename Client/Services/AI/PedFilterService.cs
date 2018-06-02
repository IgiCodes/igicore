using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Client.Utility;

namespace IgiCore.Client.Services.AI
{
	public class PedFilterService : ClientService
	{
		// https://wiki.gtanet.work/index.php?title=Peds
		public readonly List<PedHash> IllicalPeds = new List<PedHash>
		{
			// Police
			PedHash.Cop01SFY,
			PedHash.Cop01SMY,
			PedHash.Hwaycop01SMY,
			PedHash.Ranger01SFY,
			PedHash.Ranger01SMY,
			PedHash.Sheriff01SFY,
			PedHash.Sheriff01SMY,

			// Fire
			PedHash.Fireman01SMY,

			// Security
			PedHash.Security01SMM,

			// FIB
			PedHash.FibSec01,
			PedHash.FibSec01SMM,
			PedHash.CiaSec01SMM,
		};

		public override async Task Tick()
		{
			foreach (var ped in new PedList().Where(p => !p.IsPlayer && p.Model.IsValid && this.IllicalPeds.Contains((PedHash)p.Model.Hash)))
			{
				ped.Delete();
			}
		}
	}
}
