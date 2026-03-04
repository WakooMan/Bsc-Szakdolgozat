namespace GameLogic.PlayerActions
{
    public class TurnDecision : IPlayerAction
    {
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

        public TurnDecision(BuildWonder playerAction)
        {
            m_playerAction = playerAction;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return m_playerAction?.CanPerform(gameContext) ?? false;
        }
        public void DoPlayerAction(IGameContext gameContext)
        {
            m_playerAction?.DoPlayerAction(gameContext);
        }

        private readonly IPlayerAction? m_playerAction;
    }
}
