using System.Data.Entity.Migrations;
using System.Linq;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;
using Roleplay.Server.Storage;

namespace Roleplay.Server.Controllers
{
	public class CharacterController : ConfigurableController<Configuration>
	{
		public CharacterController(ILogger logger, IEventManager events, IRpcHandler rpc, Configuration configuration) : base(logger, events, rpc, configuration)
		{
			this.Rpc.Event("characters:list").On(Characters);
		}

		public void Characters(IRpcEvent e)
		{
			this.Logger.Info($"Characters: {e.User.Name}");

			using (var context = new CharacterContext())
			{
				var characters = context.Characters.Where(c => c.User.Id == e.User.Id && c.Deleted == null).OrderBy(c => c.Created).ToList();

				e.Reply(characters);
			}
		}
	}
}
