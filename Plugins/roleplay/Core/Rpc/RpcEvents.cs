namespace Roleplay.Core.Rpc
{
	public static class RpcEvents
	{
		public const string GetCharacters = "igi:user:characters";
		public const string AcceptRules = "igi:user:rules";

		public const string CharacterCreate = "igi:character:create";
		public const string CharacterLoad = "igi:character:load";
		public const string CharacterDelete = "igi:character:delete";
		public const string CharacterSave = "igi:character:save";

		public const string CharacterComponentSet = "igi:character:component:set";
		public const string CharacterPropSet = "igi:character:prop:set";
	}
}
