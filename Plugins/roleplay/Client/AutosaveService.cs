using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.SDK.Client.Services;
using IgiCore.SDK.Core.Rpc;

namespace Roleplay.Client
{
	//public class AutosaveService : Service
	//{
	//	public bool Enabled { get; protected set; } = false;

	//	//public AutosaveService()
	//	//{
	//	//	//Client.Instance.Controllers.First<CharacterController>().OnCharacterLoaded += (s, e) => this.Enable();
	//	//}

	//	//public void Enable() => this.Enabled = true;
	//	//public void Disable() => this.Enabled = false;

	//	//public override async Task Tick()
	//	//{
	//	//	if (!this.Enabled) return;

	//	//	Client.Instance.Controllers.First<CharacterController>().ActiveCharacter.Position = Game.PlayerPed.Position;

	//	//	Server.Event(RpcEvents.CharacterSave)
	//	//		.Attach(Client.Instance.Controllers.First<CharacterController>().ActiveCharacter)
	//	//		.Trigger();

	//	//	await BaseScript.Delay(1000);
	//	//}
	//}
}
