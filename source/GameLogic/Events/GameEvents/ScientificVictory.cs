namespace GameLogic.Events.GameEvents
{
    public class ScientificVictory: GameEvent
    {
        public string PlayerName { get; }

        public ScientificVictory(string playerName)
        {
            PlayerName = playerName;
        }
    }
}
