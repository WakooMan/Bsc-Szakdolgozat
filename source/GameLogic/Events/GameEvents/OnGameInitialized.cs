namespace GameLogic.Events.GameEvents
{
    public class OnGameInitialized : GameEvent
    {
        public IGameContext GameContext { get; }

        public OnGameInitialized(IGameContext gameContext)
        {
            GameContext = gameContext;
        }
    }
}
