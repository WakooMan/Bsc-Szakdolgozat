using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.Goods;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.Handlers
{
    [Export(typeof(ICostCalculator))]
    public class CostCalculator : ICostCalculator
    {
        [ImportingConstructor]
        public CostCalculator(IEventManager eventManager)
        {
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));

            m_eventManager = eventManager;
        }

        public bool CanAfford(IBuildable buildable, Player buyer, Player opponent)
        {
            int cost = GetBuildCost(buildable, buyer, opponent);
            return buyer.Money >= cost;
        }

        public int GetBuildCost(IBuildable buildable, Player buyer, Player opponent)
        {
            var missing = GetMissingGoods(buildable, buyer);
            int totalCost = 0;
            Dictionary<Type, Good> opponentGoods = opponent.Goods;

            OnBuildingCostCalculated onBuildingCostCalculated = new OnBuildingCostCalculated(buyer);
            m_eventManager.Publish(onBuildingCostCalculated);

            foreach (var cheaperBuilding in onBuildingCostCalculated.CheaperBuildings.Where(cb => cb.BuildingType == buildable.BuildingType))
            {
                int amount = cheaperBuilding.AmountOfResources;

                foreach (var good in missing)
                {
                    int used = Math.Min(amount, good.Amount);
                    good.Amount -= used;
                    amount -= used;

                    if (amount == 0) break;
                }
            }

            foreach (Good good in missing)
            {
                List<BuyGoodItem> items = onBuildingCostCalculated.BuyGoodItems.Where(item => good.GetType().Name == item.GoodType).ToList();
                int price = items.Any() ? GetDiscount(items) : 2 + (opponentGoods.ContainsKey(good.GetType()) ? opponentGoods[good.GetType()].Amount : 0);
                totalCost += price * good.Amount;
            }

            return totalCost + buildable.MoneyCost;
        }

        public List<Good> GetMissingGoods(IBuildable buildable, Player buyer)
        {
            List<Good> missing = new List<Good>();
            Dictionary<Type, Good> ownerGoods = buyer.Goods;

            foreach (Good good in buildable.GoodCost)
            {
                Good missingGood = good.Clone();
                if (ownerGoods.ContainsKey(good.GetType()))
                {
                    missingGood.Amount = Math.Max(0, missingGood.Amount - ownerGoods[good.GetType()].Amount);
                    if (missingGood.Amount > 0)
                    {
                        missing.Add(missingGood);
                    }
                }
                else
                {
                    missing.Add(missingGood);
                }
            }

            return missing;
        }

        private int GetDiscount(List<BuyGoodItem> buyGoodItems)
        {
            if (buyGoodItems.Count <= 0)
            {
                throw new InvalidOperationException("There is no discount for the player, that can be applied!");
            }

            int discount = buyGoodItems[0].MoneyCost;
            for (int i = 1; i < buyGoodItems.Count; i++)
            {
                discount = Math.Min(discount, buyGoodItems[i].MoneyCost);
            }

            return discount;
        }


        private readonly IEventManager m_eventManager;
    }
}
