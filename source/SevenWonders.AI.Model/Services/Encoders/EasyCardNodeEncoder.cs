using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;
using GameLogic.Elements.Guilds;
using GameLogic.GameStructures;
using SevenWonders.AI.Model.Services.CardTypeEncoders;
using SevenWonders.AI.Model.Services.CardTypeEncoders.Factories;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class EasyCardNodeEncoder: IEasyCardNodeEncoder
    {
        private static readonly Type[] s_goodTypes =
        [
            typeof(Clay),
            typeof(Stone),
            typeof(Wood),
            typeof(Glass),
            typeof(Papirus)
        ];

        public EasyCardNodeEncoder(IGame game, ICardTypeEncoderFactory cardTypeEncoderFactory)
        {
            m_game = game;
            m_cardTypeEncoderFactory = cardTypeEncoderFactory;
        }

        public void EncodeCardNode(List<float> vector, ICardNode cardNode, bool isAvailable, PlayerProperties ownerProperties, PlayerProperties opponentProperties)
        {
            var properties = CreateCardNodeProperties();
            properties["Exists"] = 1f;
            if (cardNode.Hidden)
            {
                properties["Visible"] = 0f;
                vector.AddRange(properties.Values);
            }
            else
            {
                properties["Visible"] = 1f;
                properties["Available"] = isAvailable ? 1f : 0f;

                int moneyCost = m_game.Context.CostCalculator.GetBuildCost(cardNode.CardObj, ownerProperties.Owner, ownerProperties.Opponent);
                var missingGoods = m_game.Context.CostCalculator.GetMissingGoods(cardNode.CardObj, ownerProperties);

                properties["MoneyCost"] = moneyCost / 100f;
                foreach (Type goodType in s_goodTypes)
                {
                    float amount = 0f;
                    foreach (var missingGood in missingGoods)
                    {
                        if (missingGood.GetType() == goodType)
                        {
                            amount += missingGood.Amount;
                        }
                    }
                    properties["Missing" + goodType.Name] = amount / 10f;
                }

                int opponentMoneyCost = m_game.Context.CostCalculator.GetBuildCost(cardNode.CardObj, opponentProperties.Owner, opponentProperties.Opponent);
                var opponentMissingGoods = m_game.Context.CostCalculator.GetMissingGoods(cardNode.CardObj, opponentProperties);

                properties["OpponentMoneyCost"] = opponentMoneyCost / 100f;
                foreach (Type goodType in s_goodTypes)
                {
                    float amount = 0f;
                    foreach (var missingGood in opponentMissingGoods)
                    {
                        if (missingGood.GetType() == goodType)
                        {
                            amount += missingGood.Amount;
                        }
                    }
                    properties["OpponentMissing" + goodType.Name] = amount / 10f;
                }

                bool isChainBuild = !string.IsNullOrEmpty(cardNode.CardObj.PreviousBuilding) &&
                                    ownerProperties.Owner.Cards.Any(c => c.Name == cardNode.CardObj.PreviousBuilding);
                properties["IsChainBuild"] = isChainBuild ? 1f : 0f;

                bool isOpponentChainBuild = !string.IsNullOrEmpty(cardNode.CardObj.PreviousBuilding) &&
                                            opponentProperties.Owner.Cards.Any(c => c.Name == cardNode.CardObj.PreviousBuilding);
                properties["IsOpponentChainBuild"] = isOpponentChainBuild ? 1f : 0f;
                properties["HasChainChild"] = cardNode.CardObj.HasChainChild ? 1f : 0f;

                ICardTypeEncoder? cardTypeEncoder = m_cardTypeEncoderFactory.Create(cardNode.CardObj.GetType());
                if (cardTypeEncoder is not null)
                {
                    cardTypeEncoder.EncodeCard(cardNode.CardObj, properties);
                }

                vector.AddRange(properties.Values);
            }
        }

        public void EncodeEmptyCardNode(List<float> vector)
        {
            var properties = CreateCardNodeProperties();
            vector.AddRange(properties.Values);
        }

        private static OrderedDictionary<string, float> CreateCardNodeProperties()
        {
            var properties = new OrderedDictionary<string, float>();
            properties.Add("Exists", 0f);
            properties.Add("Visible", 0f);
            properties.Add("Available", 0f);
            properties.Add("MoneyCost", 0f);
            properties.Add("Missing" + nameof(Clay), 0f);
            properties.Add("Missing" + nameof(Stone), 0f);
            properties.Add("Missing" + nameof(Wood), 0f);
            properties.Add("Missing" + nameof(Glass), 0f);
            properties.Add("Missing" + nameof(Papirus), 0f);
            properties.Add("OpponentMoneyCost", 0f);
            properties.Add("OpponentMissing" + nameof(Clay), 0f);
            properties.Add("OpponentMissing" + nameof(Stone), 0f);
            properties.Add("OpponentMissing" + nameof(Wood), 0f);
            properties.Add("OpponentMissing" + nameof(Glass), 0f);
            properties.Add("OpponentMissing" + nameof(Papirus), 0f);
            properties.Add("IsChainBuild", 0f);
            properties.Add("IsOpponentChainBuild", 0f);
            properties.Add("HasChainChild", 0f);
            properties.Add("CardType", 0f);

            properties.Add(nameof(VictoryPoints), 0f);

            properties.Add(nameof(Strength), 0f);

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

            properties.Add(nameof(BuilderGuild), 0f);
            properties.Add(nameof(TraderGuild), 0f);
            properties.Add(nameof(ScienceGuild), 0f);
            properties.Add(nameof(StrategistGuild), 0f);
            properties.Add(nameof(SailorGuild), 0f);
            properties.Add(nameof(MagistrateGuild), 0f);
            properties.Add(nameof(ExtortionistGuild), 0f);

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

        private readonly IGame m_game;
        private readonly ICardTypeEncoderFactory m_cardTypeEncoderFactory;
    }
}
