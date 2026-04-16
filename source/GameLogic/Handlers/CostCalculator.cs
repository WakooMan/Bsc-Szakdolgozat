using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.Goods;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public class CostCalculator : ICostCalculator
    {
        public CostCalculator(IEventManager eventManager)
        {
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));

            m_eventManager = eventManager;
        }

        public async Task<bool> CanAfford(IBuildable buildable, Player buyer, Player opponent)
        {
            int cost = await GetBuildCost(buildable, buyer, opponent);
            return buyer.Money >= cost;
        }

        public async Task<int> GetBuildCost(IBuildable buildable, Player buyer, Player opponent)
        {
            var missing = await GetMissingGoods(buildable, buyer);
            int totalCost = 0;
            IReadOnlyDictionary<Type, Good> opponentGoods = (await opponent.GetPlayerProperties()).Goods;

            OnBuildingCostCalculated onBuildingCostCalculated = new OnBuildingCostCalculated(buyer);
            await m_eventManager.PublishAsync(onBuildingCostCalculated);

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
                List<BuyGoodItem> items = onBuildingCostCalculated.BuyGoodItems.Where(item => good.GetType().Name == (item?.GoodType ?? "None")).ToList();
                int enemyGoodNumber = opponentGoods.ContainsKey(good.GetType()) ? opponentGoods[good.GetType()].Amount : 0;
                int price = items.Count > 0 ? GetDiscount(items) : 2 + enemyGoodNumber;
                totalCost += price * good.Amount;
            }

            return totalCost + buildable.MoneyCost;
        }

        public async Task<List<Good>> GetMissingGoods(IBuildable buildable, Player buyer)
        {
            List<Good> missing = new List<Good>();
            PlayerProperties playerProperties = await buyer.GetPlayerProperties();
            IReadOnlyDictionary<Type, Good> ownerGoods = playerProperties.Goods;
            IReadOnlyList<ChooseGood> chooseGoods = playerProperties.GetEffects<ChooseGood>();

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

            foreach (ChooseGood chooseGood in chooseGoods)
            {
                foreach (Good choosableGood in chooseGood.GetGoods())
                {
                    Good? good = missing.FirstOrDefault(g => g.GetType() == choosableGood.GetType());
                    if (good is not null)
                    {
                        missing.Remove(good);
                        break;
                    }
                }
            }

            return missing;
        }

        private static int GetDiscount(List<BuyGoodItem> buyGoodItems)
        {
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
