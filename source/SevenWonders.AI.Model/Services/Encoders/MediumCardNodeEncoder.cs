using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using System.Linq;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class MediumCardNodeEncoder: IMediumCardNodeEncoder
    {

        public MediumCardNodeEncoder(IGame game)
        {
            m_game = game;
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
                properties["ID"] = cardNode.CardObj.ID / 80f;
                int moneyCost = m_game.Context.CostCalculator.GetBuildCost(cardNode.CardObj, ownerProperties.Owner, ownerProperties.Opponent);

                properties["MoneyCost"] = moneyCost / 100f;
                int opponentMoneyCost = m_game.Context.CostCalculator.GetBuildCost(cardNode.CardObj, opponentProperties.Owner, opponentProperties.Opponent);
                properties["OpponentMoneyCost"] = opponentMoneyCost / 100f;
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
                        properties.Add(nameof(BlueCard), 1f);
                        properties.Add(nameof(VictoryPoints), blueCard.Point.Points / 60f);
                        break;
                    case RedCard redCard:
                        properties.Add(nameof(RedCard), 1f);
                        properties.Add(nameof(Strength), redCard.Strength.Points / 30f);
                        break;
                    case BrownCard brownCard:
                        properties.Add(nameof(BrownCard), 1f);
                        break;
                    case YellowCard yellowCard:
                        properties.Add(nameof(YellowCard), 1f);
                        properties.Add(nameof(VictoryPoints), yellowCard.Effects.OfType<VictoryPoints>().Sum(p => p.Points));
                        properties.Add(nameof(Strength), yellowCard.Effects.OfType<Strength>().Sum(p => p.Points));
                        break;
                    case PurpleCard purpleCard:
                        properties.Add(nameof(PurpleCard), 1f);
                        break;
                    case GreenCard greenCard:
                        properties.Add(nameof(GreenCard), 1f);
                        properties.Add(nameof(VictoryPoints), greenCard.Point.Points / 60f);

                        var disciplineName = greenCard.Discipline.GetType().Name;
                        if (properties.ContainsKey(disciplineName))
                        {
                            properties[disciplineName] = 1f;
                        }
                        break;
                    case GrayCard grayCard:
                        properties.Add(nameof(GrayCard), 1f);
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
            properties.Add("Exists", 0f);
            properties.Add("Visible", 0f);
            properties.Add("Available", 0f);
            properties.Add("MoneyCost", 0f);
            properties.Add("OpponentMoneyCost", 0f);
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
            properties.Add(nameof(VictoryPoints), 0f);
            properties.Add(nameof(Strength), 0f);
            properties.Add(nameof(Building), 0f);
            properties.Add(nameof(Geography), 0f);
            properties.Add(nameof(Healing), 0f);
            properties.Add(nameof(Mechanics), 0f);
            properties.Add(nameof(Physics), 0f);
            properties.Add(nameof(Trading), 0f);
            properties.Add(nameof(Writing), 0f);
            return properties;
        }

        private readonly IGame m_game;
    }
}
