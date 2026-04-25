using GameLogic.Elements;
using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class MediumPlayerEncoder: IMediumPlayerEncoder
    {
        public MediumPlayerEncoder(IEffectEncoder effectEncoder)
        {
            m_effectEncoder = effectEncoder;
        }

        public void EncodePlayer(List<float> vector, PlayerProperties playerProperties)
        {
            var playerEncodesProps = CreatePlayerProperties();
            playerEncodesProps.Add("Money", playerProperties.Owner.Money / 100f);
            playerEncodesProps.Add(nameof(VictoryPoints), playerProperties.VictoryPoints / 60f);
            playerEncodesProps.Add(nameof(Strength), playerProperties.Strength / 30f);

            for (int i = 0; i < 4; i++)
            {
                var wonders = playerProperties.Owner.Wonders;
                playerEncodesProps.Add($"Wonder{i}Built", wonders[i].HasBeenBuilt ? 1f : 0f);
            }

            Type[] goodTypes = [typeof(Clay), typeof(Stone), typeof(Wood), typeof(Glass), typeof(Papirus)];
            foreach (var goodType in goodTypes)
            {
                playerEncodesProps.Add(goodType.Name, playerProperties.Goods.TryGetValue(goodType, out var good) ? good.Amount / 10f : 0f);
            }

            Type[] disciplineTypes = [typeof(Building), typeof(Geography), typeof(Healing), typeof(Mechanics), typeof(Physics), typeof(Trading), typeof(Writing)];
            foreach (var disciplineType in disciplineTypes)
            {
                playerEncodesProps.Add(disciplineType.Name, playerProperties.Disciplines.TryGetValue(disciplineType, out var count) ? count / 10f : 0f);
            }

            Type[] cardTypes = [typeof(BrownCard), typeof(BlueCard), typeof(GrayCard), typeof(GreenCard), typeof(PurpleCard), typeof(RedCard), typeof(YellowCard)];
            foreach (var cardType in cardTypes)
            {
                playerEncodesProps.Add(cardType.Name, playerProperties.Owner.Cards.Count(c => c.GetType() == cardType) / 10f);
            }

            foreach (Effect effect in playerProperties.Effects)
            {
                m_effectEncoder.EncodeEffect(effect, playerEncodesProps);
            }
        }

        private static OrderedDictionary<string, float> CreatePlayerProperties()
        {
            var properties = new OrderedDictionary<string, float>();
            properties.Add("Money", 0f);
            properties.Add(nameof(VictoryPoints), 0f);
            properties.Add(nameof(Strength), 0f);
            properties.Add("Wonder0Built", 0f);
            properties.Add("Wonder1Built", 0f);
            properties.Add("Wonder2Built", 0f);
            properties.Add("Wonder3Built", 0f);
            properties.Add(nameof(BrownCard), 0f);
            properties.Add(nameof(BlueCard), 0f);
            properties.Add(nameof(GrayCard), 0f);
            properties.Add(nameof(GreenCard), 0f);
            properties.Add(nameof(PurpleCard), 0f);
            properties.Add(nameof(RedCard), 0f);
            properties.Add(nameof(YellowCard), 0f);

            properties.Add(nameof(Building), 0f);
            properties.Add(nameof(Geography), 0f);
            properties.Add(nameof(Healing), 0f);
            properties.Add(nameof(Mechanics), 0f);
            properties.Add(nameof(Physics), 0f);
            properties.Add(nameof(Trading), 0f);
            properties.Add(nameof(Writing), 0f);

            properties.Add(nameof(Papirus), 0f);
            properties.Add(nameof(Glass), 0f);
            properties.Add(nameof(Clay), 0f);
            properties.Add(nameof(Stone), 0f);
            properties.Add(nameof(Wood), 0f);

            properties.Add(nameof(BuyGoods), 0f);
            properties.Add(nameof(ChooseGood), 0f);
            properties.Add(nameof(GetMoney), 0f);
            properties.Add(nameof(GetMoneyForCard) + nameof(BlueCard), 0f);
            properties.Add(nameof(GetMoneyForCard) + nameof(BrownCard), 0f);
            properties.Add(nameof(GetMoneyForCard) + nameof(GrayCard), 0f);
            properties.Add(nameof(GetMoneyForCard) + nameof(GreenCard), 0f);
            properties.Add(nameof(GetMoneyForCard) + nameof(PurpleCard), 0f);
            properties.Add(nameof(GetMoneyForCard) + nameof(RedCard), 0f);
            properties.Add(nameof(GetMoneyForCard) + nameof(YellowCard), 0f);
            properties.Add(nameof(GetMoneyForWonders), 0f);
            properties.Add(nameof(EnemyLoseMoney), 0f);
            properties.Add(nameof(BuildFreeFromDroppedCards), 0f);
            properties.Add(nameof(ChooseDevelopment), 0f);
            properties.Add(nameof(DropEnemyCard), 0f);
            properties.Add(nameof(NewTurn), 0f);
            properties.Add(nameof(Mathematics), 0f);
            properties.Add(nameof(MoneyOnChainBuild), 0f);
            properties.Add(nameof(PlusStrengthOnRedCardBuild), 0f);
            properties.Add(nameof(CheaperBuilding) + nameof(BlueCard), 0f);
            properties.Add(nameof(CheaperBuilding) + nameof(BrownCard), 0f);
            properties.Add(nameof(CheaperBuilding) + nameof(GrayCard), 0f);
            properties.Add(nameof(CheaperBuilding) + nameof(GreenCard), 0f);
            properties.Add(nameof(CheaperBuilding) + nameof(PurpleCard), 0f);
            properties.Add(nameof(CheaperBuilding) + nameof(RedCard), 0f);
            properties.Add(nameof(CheaperBuilding) + nameof(YellowCard), 0f);
            properties.Add(nameof(Law), 0f);
            properties.Add(nameof(Economics), 0f);
            properties.Add(nameof(Teology), 0f);
            return properties;
        }

        private readonly IEffectEncoder m_effectEncoder;
    }
}
