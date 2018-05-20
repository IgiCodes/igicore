namespace IgiCore.SDK.Core.Rpc
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

		public const string CharacterComponentSet = "igi:character:component:set";
		public const string CharacterPropSet = "igi:character:prop:set";
		
		public const string BankAtmWithdraw = "igi:bank:atm:withdraw";
		public const string BankBranchWithdraw = "igi:bank:branch:withdraw";
		public const string BankBranchDeposit = "igi:bank:branch:withdraw";
		public const string BankBranchTransfer = "igi:bank:branch:transfer";
		public const string BankOnlineTransfer = "igi:bank:online:transfer";

		public const string CarCreate = "igi:car:create";
		public const string CarSpawn = "igi:car:spawn";
		public const string CarSave = "igi:car:save";
		public const string CarTransfer = "igi:car:transfer";
		public const string CarClaim = "igi:car:claim";
		public const string CarUnclaim = "igi:car:unclaim";

		public const string BikeSpawn = "igi:bike:spawn";
		public const string BikeSave = "igi:bike:save";
		public const string BikeTransfer = "igi:bike:transfer";
		public const string BikeClaim = "igi:bike:claim";
		public const string BikeUnclaim = "igi:bike:unclaim";
	}
}
