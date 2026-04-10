namespace GameLogic.Events.GameEvents
{
    public class MilitaryVictory: GameEvent
    {
        public string PlayerName { get; }

        public MilitaryVictory(string playerName)
        {
            PlayerName = playerName;
        }
    }
}
