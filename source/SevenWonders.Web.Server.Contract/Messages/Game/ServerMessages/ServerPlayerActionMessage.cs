namespace SevenWonders.Web.Server.Contract.Messages.Game.ServerMessages
{
    public class ServerPlayerActionMessage: GameServerMessage
    {
        public string PlayerName { get; set; }
        public int ActionId { get; set; }

        public ServerPlayerActionMessage(): base(false, "")
        {
            PlayerName = string.Empty;
            ActionId = -1;
        }

        public ServerPlayerActionMessage(string playerName, int actionId): base(true, "")
        {
            PlayerName = playerName;
            ActionId = actionId;
        }
    }
}
