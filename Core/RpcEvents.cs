namespace IgiCore.Core
{
	public static class RpcEvents
	{
		public const string GetServerInformation = "igi:client:ready";
		public const string ClientDisconnect = "igi:client:disconnect";

		public const string GetUser = "igi:user:load";
		public const string GetCharacters = "igi:user:characters";
		public const string AcceptRules = "igi:user:rules";
		
		public const string CharacterCreate = "igi:character:create";
		public const string CharacterLoad = "igi:character:load";
		public const string CharacterDelete = "igi:character:delete";
		public const string CharacterSave = "igi:character:save";
	}
}
