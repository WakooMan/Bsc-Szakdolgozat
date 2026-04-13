namespace WebServer.Contract.Messages.Game.ServerMessages
{
    public class PlayerActionResponseMessage : GameServerMessage
    {
        public int ActionId { get; set; }
        public PlayerActionResponseMessage(): base() { ActionId = -1; }
        public PlayerActionResponseMessage(bool isSuccess, string message, int actionId) : base(isSuccess, message)
        {
            ActionId = actionId;
        }
    }
}
