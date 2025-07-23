using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.PlayerTurnStates
{
    public class MakeActionDecision : IPlayerTurnState
    {
        public bool GoToPrevState { get; set; }

        public MakeActionDecision(IPlayerActionReceiver playerActionReceiver, Player player, ICardComposition composition)
        {
            m_playerActionReceiver = playerActionReceiver;
            m_player = player;
            m_composition = composition;
            GoToPrevState = false;
        }

        public void ExecuteTurnState(IEventManager eventManager)
        {
            List<IPlayerAction> playerActions =
            [
                new UnpickCard(eventManager, this, m_player), new BuildCard(m_composition, m_player), new SellCard(m_composition, m_player),
                .. m_player.Wonders.Select(wonder => new BuildWonder(m_composition, wonder, m_player)),
            ];

            m_playerActionReceiver.ReceivePlayerAction(m_player, playerActions).DoPlayerAction();
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return GoToPrevState ? new PickCardState(m_playerActionReceiver, m_player, m_composition) : new EndTurn();
        }

        private readonly Player m_player;
        private readonly ICardComposition m_composition;
        private readonly IPlayerActionReceiver m_playerActionReceiver;
    }
}
