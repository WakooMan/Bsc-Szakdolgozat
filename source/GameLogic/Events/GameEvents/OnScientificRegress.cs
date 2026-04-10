namespace GameLogic.Events.GameEvents
{
    public class OnScientificRegress: GameEvent
    {
        public int PlayerId { get; }
        public IReadOnlyDictionary<Type, int> Disciplines { get; }

        public OnScientificRegress(int playerId, IReadOnlyDictionary<Type, int> disciplines)
        {
            PlayerId = playerId;
            Disciplines = disciplines;
        }
    }
}
