using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.PlayerTurnStates;

namespace GameLogic.PlayerActions
{
    public class UnpickCard : IPlayerAction
    {
        public UnpickCard(IEventManager eventManager, MakeActionDecision makeActionDecision, Player player)
        {
            m_eventManager = eventManager;
            m_player = player;
            m_makeActionDecision = makeActionDecision;
        }

        public bool CanPerform()
        {
            return m_player.PickedCard is not null;
        }

        public void DoPlayerAction()
        {
            m_player.PickedCard = null;
            m_makeActionDecision.GoToPrevState = true;
            m_eventManager.Publish(GameEventType.CardUnpicked, new EventArgs());
        }

        private readonly Player m_player;
        private readonly IEventManager m_eventManager;
        private readonly MakeActionDecision m_makeActionDecision;
    }
}
