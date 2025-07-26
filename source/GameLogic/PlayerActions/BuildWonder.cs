using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Handlers;

namespace GameLogic.PlayerActions
{
    public class BuildWonder : IPlayerAction
    {

        public BuildWonder(IEventManager eventManager, ICostCalculator costCalculator, ICardComposition composition, Wonder wonder, Player player, Player opponent)
        {
            m_eventManager = eventManager;
            m_costCalculator = costCalculator;
            m_composition = composition;
            m_wonder = wonder;
            m_player = player;
            m_opponent = opponent;
        }

        public void DoPlayerAction()
        {
            if (m_player.PickedCard is null)
            {
                throw new InvalidOperationException($"{m_player.Name} player's picked card is null, {nameof(BuildWonder)} action cannot be performed!");
            }

            m_composition.RemoveCard(m_player.PickedCard);
            m_player.Money -= m_costCalculator.GetBuildCost(m_wonder, m_player, m_opponent);
            m_wonder.HasBeenBuilt = true;
            m_eventManager.Publish(GameEventType.WonderBuilt, new OnWonderBuilt(m_player, m_wonder));
            m_wonder.OnBuilt(m_player, m_eventManager);
        }

        public bool CanPerform()
        {
            if (!m_player.Wonders.Contains(m_wonder) || m_wonder.HasBeenBuilt || m_player.PickedCard is null)
            {
                return false;
            }

            return m_costCalculator.CanAfford(m_wonder, m_player, m_opponent);
        }

        private readonly IEventManager m_eventManager;
        private readonly ICostCalculator m_costCalculator;
        private readonly Wonder m_wonder;
        private readonly ICardComposition m_composition;
        private readonly Player m_player;
        private readonly Player m_opponent;
    }
}
