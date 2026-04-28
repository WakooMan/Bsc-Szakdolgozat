namespace GameLogic.PlayerActions
{
    public class TurnDecision : IPlayerAction
    {
        public string Name => m_playerAction?.Name ?? throw new InvalidOperationException("No player action selected.");
        public int Id => m_playerAction?.Id ?? 12;
        public IPlayerAction? PlayerAction => m_playerAction;

        public TurnDecision()
        {
            m_playerAction = null;
        }

        public TurnDecision(UnpickCard playerAction)
        {
            m_playerAction = playerAction;
        }

        public TurnDecision(BuildCard playerAction)
        {
            m_playerAction = playerAction;
        }

        public TurnDecision(SellCard playerAction)
        {
            m_playerAction = playerAction;
        }

        public TurnDecision(BuildWonderProcess playerAction)
        {
            m_playerAction = playerAction;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return m_playerAction?.CanPerform(gameContext) ?? false;
        }
        public bool DoPlayerAction(IGameContext gameContext)
        {
            return m_playerAction?.DoPlayerAction(gameContext) ?? false;
        }

        private readonly IPlayerAction? m_playerAction;
    }
}
