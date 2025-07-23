using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class SellCard : IPlayerAction
    {
        public SellCard(ICardComposition composition, Player player)
        {
            m_composition = composition;
            m_player = player;
        }

        public void DoPlayerAction()
        {
            m_composition.RemoveCard(m_player.PickedCard);
            m_player.Money += 2 + m_player.Cards.Where(card => card is YellowCard).Count();
        }

        public bool CanPerform()
        {
            return true;
        }

        private readonly ICardComposition m_composition;
        private readonly Player m_player;
    }
}
