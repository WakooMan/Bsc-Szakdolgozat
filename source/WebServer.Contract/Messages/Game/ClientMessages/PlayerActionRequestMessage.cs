namespace WebServer.Contract.Messages.Game.ClientMessages
{
    public class PlayerActionRequestMessage: GameClientMessage
    {
        public int ActionId { get; set; }

        public PlayerActionRequestMessage()
        {
            ActionId = -1;
        }

        public PlayerActionRequestMessage(int actionId)
        {
            ActionId = actionId;
        }
    }
}
