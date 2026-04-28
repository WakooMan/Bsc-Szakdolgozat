using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using SevenWonders.AI.Model.Services.Encoders.Structs;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class HardCardNodeEncoder : IHardCardNodeEncoder
    {
        public HardCardNodeEncoder(IGame game, IHardEncoderHelper helper, ICardEffectAnalyzer cardEffectAnalyzer)
        {
            m_game = game;
            m_helper = helper;
            m_cardEffectAnalyzer = cardEffectAnalyzer;
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
                int moneyCost = m_game.Context.CostCalculator.GetBuildCost(cardNode.CardObj, ownerProperties.Owner, ownerProperties.Opponent);
                bool canAfford = m_game.Context.CostCalculator.CanAfford(cardNode.CardObj, ownerProperties.Owner, ownerProperties.Opponent);
                int opponentMoneyCost = m_game.Context.CostCalculator.GetBuildCost(cardNode.CardObj, opponentProperties.Owner, opponentProperties.Opponent);
                bool isChainBuild = !string.IsNullOrEmpty(cardNode.CardObj.PreviousBuilding) &&
                    ownerProperties.Owner.Cards.Any(c => c.Name == cardNode.CardObj.PreviousBuilding);
                bool isOpponentChainBuild = !string.IsNullOrEmpty(cardNode.CardObj.PreviousBuilding) &&
                            opponentProperties.Owner.Cards.Any(c => c.Name == cardNode.CardObj.PreviousBuilding);

                CardEffectAnalysis analysis = m_cardEffectAnalyzer.AnalyzeCardEffects(cardNode.CardObj, ownerProperties, moneyCost);
                if (properties.ContainsKey(cardNode.CardObj.BuildingType))
                {
                    properties[cardNode.CardObj.BuildingType] = 1f;
                }

                int currentCount = 0;
                int opponentCount = 0;
                ScienceAnalysis scienceAfter = new ScienceAnalysis();
                if (analysis.ScienceDiscipline is not null)
                {
                    currentCount = ownerProperties.Disciplines.TryGetValue(analysis.ScienceDiscipline, out var c) ? c : 0;
                    scienceAfter = m_helper.AnalyzeScienceWithAdded(ownerProperties, analysis.ScienceDiscipline);
                    opponentCount = opponentProperties.Disciplines.TryGetValue(analysis.ScienceDiscipline, out var oc) ? oc : 0;
                }

                int moneyAfterBuild = Math.Max(0, ownerProperties.Owner.Money + analysis.DeltaCoins);
                int opponentMoneyAfterBuild = Math.Max(0, opponentProperties.Owner.Money - opponentMoneyCost);

                float currentRatio = m_helper.CalculateAffordableCardsRatio(ownerProperties);
                ICardComposition composition = m_game.Context.AgeHandler.CurrentAge.Composition;
                int affordableAfterBuild = 0;
                int availableCount = 0;
                foreach (ICardNode node in composition.AvailableCards)
                {
                    if (node.Hidden || node == cardNode) continue;
                    availableCount++;
                    int nodeCost = m_game.Context.CostCalculator.GetBuildCost(node.CardObj, ownerProperties.Owner, ownerProperties.Opponent);
                    if (moneyAfterBuild >= nodeCost)
                    {
                        affordableAfterBuild++;
                    }
                }
                float afterRatio = availableCount > 0 ? affordableAfterBuild / (float)availableCount : 0f;

                MilitaryAnalysis military = m_helper.AnalyzeMilitary(ownerProperties, opponentProperties);
                int currentPos = 0;
                int newPos = 0;
                if (military.BoardLength > 0)
                {
                    int newStrengthDiff = military.StrengthDiff + analysis.DeltaStrength;
                    currentPos = Math.Clamp(military.BoardMiddle + military.StrengthDiff, 0, military.BoardLength - 1);
                    newPos = Math.Clamp(military.BoardMiddle + newStrengthDiff, 0, military.BoardLength - 1);
                }

                properties["IsPlayable"] = 1f;
                properties["ID"] = cardNode.CardObj.ID / 80f;
                properties["MoneyCost"] = moneyCost / 100f;
                properties["CanAfford"] = canAfford ? 1f : 0f;
                properties["OpponentMoneyCost"] = opponentMoneyCost / 100f;
                properties["OpponentCanAfford"] = m_game.Context.CostCalculator.CanAfford(cardNode.CardObj, opponentProperties.Owner, opponentProperties.Opponent) ? 1f : 0f;
                properties["IsChainBuild"] = isChainBuild ? 1f : 0f;
                properties["IsOpponentChainBuild"] = isOpponentChainBuild ? 1f : 0f;
                properties["HasChainChild"] = cardNode.CardObj.HasChainChild ? 1f : 0f;
                properties[$"delta{nameof(VictoryPoints)}"] = analysis.DeltaVP / 60f;
                properties["deltaCoins"] = analysis.DeltaCoins / 100f;
                if (analysis.ScienceDiscipline is not null)
                {
                    properties["deltaScience_Progress"] = (currentCount + 1) / 10f;
                    properties["science_completion_potential"] = scienceAfter.CompleteSets / 5f;
                    properties["Science_threat_to_opponent"] = (opponentCount + 1) / 10f;
                }
                properties["deltaResource_Flex"] = analysis.DeltaResourceCount / 10f;
                properties["deltaAction_Budget"] = moneyAfterBuild / 100f;
                properties["deltaOpponent_Action_Budget"] = opponentMoneyAfterBuild / 100f;
                properties["deltaAffordable_Cards_Ratio"] = afterRatio - currentRatio;
                properties["future_cost_reduction_estimate"] = analysis.FutureCostReduction;
                if (military.BoardLength > 0)
                {
                    properties["military_track_delta"] = (newPos - currentPos) / (float)military.BoardMiddle;
                    properties["military_pressure_delta"] = analysis.DeltaStrength / 30f;
                    properties["military_win_proximity"] = newPos / (float)(military.BoardLength - 1);
                }
                properties["denial_value"] = analysis.DenialValue;

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
            properties.Add("deltaAffordable_Cards_Ratio", 0f);
            properties.Add("future_cost_reduction_estimate", 0f);
            properties.Add("military_track_delta", 0f);
            properties.Add("military_pressure_delta", 0f);
            properties.Add("military_win_proximity", 0f);
            properties.Add("denial_value", 0f);

            return properties;
        }

        private readonly IGame m_game;
        private readonly IHardEncoderHelper m_helper;
        private readonly ICardEffectAnalyzer m_cardEffectAnalyzer;
    }
}
