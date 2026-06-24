namespace SevenWonders.Web.Server.Contract.Messages.Game.ClientMessages
{
    public class PlayerActionRequestMessage: GameClientMessage
    {
        public string PlayerName { get; set; }
        public int ActionId { get; set; }
        public List<int> Actions { get; set; }

        public PlayerActionRequestMessage()
        {
            PlayerName = string.Empty;
            ActionId = -1;
            Actions = new List<int>();
        }

        public PlayerActionRequestMessage(string playerName, int actionId, List<int> actions)
        {
            PlayerName = playerName;
            ActionId = actionId;
            Actions = actions;
        }
    }
}
