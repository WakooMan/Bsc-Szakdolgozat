using GameLogic.Elements;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class BuildWonder : IPlayerAction
    {

        public BuildWonder(IEventManager eventManager, ICardComposition composition, Wonder wonder, Player player)
        {
            m_eventManager = eventManager;
            m_composition = composition;
            m_wonder = wonder;
            m_player = player;
        }

        public void DoPlayerAction()
        {
            m_composition.RemoveCard(m_player.PickedCard);
            m_wonder.HasBeenBuilt = true;
        }

        public bool CanPerform()
        {
            if (!m_player.Wonders.Contains(m_wonder) || m_wonder.HasBeenBuilt)
            {
                return false;
            }
            OnBuildingCostCalculated buildingCostCalculated = new OnBuildingCostCalculated(m_player);
            m_eventManager.Publish(GameEventType.BuildingCostCalculated, buildingCostCalculated);

            List<Good> goodsNeeded = m_wonder.GoodCost.Select(good => good.Clone()).ToList();
            foreach (Good good in m_player.Goods)
            {
                if (goodsNeeded.Contains(good))
                {
                    goodsNeeded.Remove(good);
                }
            }

            //foreach (Good good in buildingCostCalculated.AdditionalGoods)
            //{

            //}

            return goodsNeeded.Count <= 0;
        }

        private readonly IEventManager m_eventManager;
        private readonly Wonder m_wonder;
        private readonly ICardComposition m_composition;
        private readonly Player m_player;
    }
}
