using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class SellCard : IPlayerAction
    {
        public SellCard(ICardNode card, IAgeBase age, Player player)
        {
            m_card = card;
            m_age = age;
            m_player = player;
        }

        public void DoPlayerAction()
        {
            m_age.Composition.RemoveCard(m_card);
            m_player.Money += 2 + m_player.Cards.Where(card => card is YellowCard).Count();
        }

        public bool CanPerform()
        {
            return true;
        }

        private readonly ICardNode m_card;
        private readonly IAgeBase m_age;
        private readonly Player m_player;
    }
}
