using GameLogic.Elements;
using GameLogic.Elements.Goods;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class BuildCard : IPlayerAction
    {
        public BuildCard(ICardComposition composition, Player player)
        {
            m_composition = composition;
            m_player = player;
        }

        public void DoPlayerAction()
        {
            m_composition.RemoveCard(m_player.PickedCard);
            m_player.Cards.Add(m_player.PickedCard.CardObj);
        }

        public bool CanPerform()
        {
            List<Good> goodsNeeded = m_player.PickedCard.CardObj.GoodCost.Select(good => good.Clone()).ToList();
            foreach (Good good in m_player.Goods)
            {
                if(goodsNeeded.Contains(good))
                {
                    goodsNeeded.Remove(good);
                }
            }
            return goodsNeeded.Count <= 0;
        }

        private readonly ICardComposition m_composition;
        private readonly Player m_player;
    }
}
