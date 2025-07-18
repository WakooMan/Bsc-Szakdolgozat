using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.Goods;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class BuildCard : IPlayerAction
    {
        public BuildCard(ICardNode card, IAgeBase age, Player player)
        {
            m_card = card;
            m_age = age;
            m_player = player;
        }

        public void DoPlayerAction()
        {
            m_age.Composition.RemoveCard(m_card);
            m_player.Cards.Add(m_card.CardObj);
        }

        public bool CanPerform()
        {
            List<Good> goodsNeeded = m_card.CardObj.GoodCost.Select(good => good.Clone()).ToList();
            foreach (Good good in m_player.Goods)
            {
                goodsNeeded.Remove(goodsNeeded.Where(g => g.GetType() == good.GetType()).FirstOrDefault());
            }
            return goodsNeeded.Count <= 0;
        }

        private ICardNode m_card;
        private IAgeBase m_age;
        private Player m_player;
    }
}
