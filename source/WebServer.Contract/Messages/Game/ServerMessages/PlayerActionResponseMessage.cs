namespace WebServer.Contract.Messages.Game.ServerMessages
{
    public class PlayerActionResponseMessage : GameServerMessage
    {
        public string PlayerName { get; set; }
        public int ActionId { get; set; }
        public PlayerActionResponseMessage(): base() { PlayerName = string.Empty; ActionId = -1; }
        public PlayerActionResponseMessage(string message) : base(false, message)
        {
            PlayerName = string.Empty;
            ActionId = -1;
        }

        public PlayerActionResponseMessage(string playerName, int actionId) : base(true, "Success")
        {
            PlayerName = playerName;
            ActionId = actionId;
        }
    }
}
