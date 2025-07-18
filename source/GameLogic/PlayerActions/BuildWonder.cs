using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class BuildWonder : IPlayerAction
    {

        public BuildWonder(ICardNode card, IAgeBase age, Wonder wonder, Player player)
        {
            m_card = card;
            m_age = age;
            m_wonder = wonder;
            m_player = player;
        }

        public void DoPlayerAction()
        {
            m_age.Composition.RemoveCard(m_card);
            m_wonder.HasBeenBuilt = true;
        }

        public bool CanPerform()
        {
            return m_player.Wonders.Contains(m_wonder) && !m_wonder.HasBeenBuilt;
        }

        private readonly ICardNode m_card;
        private readonly Wonder m_wonder;
        private readonly IAgeBase m_age;
        private readonly Player m_player;
    }
}
