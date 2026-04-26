using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class HardCardNodeEncoder : IHardCardNodeEncoder
    {
        public HardCardNodeEncoder(IGame game)
        {
            m_game = game;
        }

        public void EncodeCardNode(List<float> vector, ICardNode cardNode, bool isAvailable, PlayerProperties ownerProperties, PlayerProperties opponentProperties)
        {
            var properties = CreateCardNodeProperties();
            if (cardNode.Hidden)
            {
                vector.AddRange(properties.Values);
            }
            else
            {
                properties["IsPlayable"] = 1f;
                properties["Available"] = isAvailable ? 1f : 0f;
                properties["ID"] = cardNode.CardObj.ID / 80f;

                int moneyCost = m_game.Context.CostCalculator.GetBuildCost(cardNode.CardObj, ownerProperties.Owner, ownerProperties.Opponent);
                properties["MoneyCost"] = moneyCost / 100f;
                properties["CanAfford"] = m_game.Context.CostCalculator.CanAfford(cardNode.CardObj, ownerProperties.Owner, ownerProperties.Opponent) ? 1f : 0f;

                int opponentMoneyCost = m_game.Context.CostCalculator.GetBuildCost(cardNode.CardObj, opponentProperties.Owner, opponentProperties.Opponent);
                properties["OpponentMoneyCost"] = opponentMoneyCost / 100f;
                properties["OpponentCanAfford"] = m_game.Context.CostCalculator.CanAfford(cardNode.CardObj, opponentProperties.Owner, opponentProperties.Opponent) ? 1f : 0f;

                bool isChainBuild = !string.IsNullOrEmpty(cardNode.CardObj.PreviousBuilding) &&
                                    ownerProperties.Owner.Cards.Any(c => c.Name == cardNode.CardObj.PreviousBuilding);
                properties["IsChainBuild"] = isChainBuild ? 1f : 0f;
                bool isOpponentChainBuild = !string.IsNullOrEmpty(cardNode.CardObj.PreviousBuilding) &&
                                            opponentProperties.Owner.Cards.Any(c => c.Name == cardNode.CardObj.PreviousBuilding);
                properties["IsOpponentChainBuild"] = isOpponentChainBuild ? 1f : 0f;
                properties["HasChainChild"] = cardNode.CardObj.HasChainChild ? 1f : 0f;
        
                switch (cardNode.CardObj)
                {
                    case BlueCard blueCard:
                        properties[nameof(BlueCard)] = 1f;
                        properties[nameof(VictoryPoints)] = blueCard.Point.Points / 60f;
                        break;
                    case RedCard redCard:
                        properties[nameof(RedCard)] = 1f;
                        properties[nameof(Strength)] = redCard.Strength.Points / 30f;
                        break;
                    case BrownCard brownCard:
                        properties[nameof(BrownCard)] = 1f;
                        foreach (var resource in brownCard.ProducedResources)
                        {
                            if (properties.ContainsKey(resource.GetType().Name))
                            {
                                properties[resource.GetType().Name] = resource.Amount / 10f;
                            }
                        }
                        break;
                    case GrayCard grayCard:
                        properties[nameof(GrayCard)] = 1f;
                        foreach (var product in grayCard.CreatedProducts)
                        {
                            if (properties.ContainsKey(product.GetType().Name))
                            {
                                properties[product.GetType().Name] = product.Amount / 10f;
                            }
                        }
                        break;
                    case GreenCard greenCard:
                        properties[nameof(GreenCard)] = 1f;
                        properties[nameof(VictoryPoints)] = greenCard.Point.Points / 60f;
                        var disciplineName = greenCard.Discipline.GetType().Name;
                        if (properties.ContainsKey(disciplineName))
                        {
                            properties[disciplineName] = 1f;
                        }
                        break;
                    case YellowCard yellowCard:
                        properties[nameof(YellowCard)] = 1f;
                        properties[nameof(VictoryPoints)] = yellowCard.Effects.OfType<VictoryPoints>().Sum(p => p.Points) / 60f;
                        properties[nameof(Strength)] = yellowCard.Effects.OfType<Strength>().Sum(p => p.Points) / 30f;
                        properties[nameof(GetMoney)] = yellowCard.Effects.OfType<GetMoney>().Sum(p => p.Money) / 10f;
                        break;
                    case PurpleCard purpleCard:
                        properties[nameof(PurpleCard)] = 1f;
                        properties[purpleCard.GuildObj.GetType().Name] = 1f;
                        break;
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
            properties.Add("ID", 0f);
            properties.Add("IsPlayable", 0f);
            properties.Add("MoneyCost", 0f);
            properties.Add("CanAfford", 0f);
            properties.Add("OpponentMoneyCost", 0f);
            properties.Add("OpponentCanAfford", 0f);
            properties.Add("IsChainBuild", 0f);
            properties.Add("IsOpponentChainBuild", 0f);
            properties.Add("HasChainChild", 0f);

            properties.Add(nameof(BrownCard), 0f);
            properties.Add(nameof(BlueCard), 0f);
            properties.Add(nameof(GrayCard), 0f);
            properties.Add(nameof(GreenCard), 0f);
            properties.Add(nameof(PurpleCard), 0f);
            properties.Add(nameof(RedCard), 0f);
            properties.Add(nameof(YellowCard), 0f);

            properties.Add($"delta{nameof(VictoryPoints)}", 0f);
            properties.Add("deltaCoins", 0f);
            properties.Add("deltaScience_Progress", 0f);
            properties.Add("science_completion_potential", 0f);
            properties.Add("Science_threat_to_opponent", 0f);
            properties.Add("deltaResource_Flex", 0f);
            properties.Add("deltaAction_Budget", 0f);
            properties.Add("deltaOpponent_Action_Budget", 0f);
            properties.Add("tempo_gain", 0f);
            properties.Add("deltaAffordable_Cards_Ratio", 0f);
            properties.Add("future_cost_reduction_estimate", 0f);
            properties.Add("military_track_delta", 0f);
            properties.Add("military_pressure_delta", 0f);
            properties.Add("military_win_proximity", 0f);
            properties.Add("future_income_rate", 0f);
            properties.Add("economic_scaling_potential", 0f);

            return properties;
        }

        private readonly IGame m_game;
    }
}
