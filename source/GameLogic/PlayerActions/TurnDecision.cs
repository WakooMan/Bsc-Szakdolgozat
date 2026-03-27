namespace GameLogic.PlayerActions
{
    public class TurnDecision : IPlayerAction
    {
        public string Name => m_playerAction?.Name ?? throw new InvalidOperationException("No player action selected.");
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

        public async Task<bool> CanPerform(IGameContext gameContext)
        {
            return await (m_playerAction?.CanPerform(gameContext) ?? Task.FromResult(false));
        }
        public async Task DoPlayerAction(IGameContext gameContext)
        {
            await (m_playerAction?.DoPlayerAction(gameContext) ?? Task.CompletedTask);
        }

        private readonly IPlayerAction? m_playerAction;
    }
}
