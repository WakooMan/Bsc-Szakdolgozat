using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.Wonders;
using SevenWonders.AI.Model.Services.Encoders.Structs;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class HardPlayerEncoder : IHardPlayerEncoder
    {
        public HardPlayerEncoder(IHardEncoderHelper helper, IGame game)
        {
            m_helper = helper;
            m_game = game;
        }

        public void EncodePlayer(List<float> vector, PlayerProperties playerProperties)
        {
            var properties = CreatePlayerProperties();

            int vpFromWonders = 0;
            int strengthFromWonders = 0;
            int coinsFromWonders = 0;
            int remainingWonderVP = 0;
            int remainingWonderMilitary = 0;
            float remainingWonderEconomic = 0f;
            float remainingWonderScience = 0f;
            float remainingExtraTurn = 0f;

            foreach (Wonder wonder in playerProperties.Owner.Wonders)
            {
                if (wonder.HasBeenBuilt)
                {
                    vpFromWonders += wonder.Effects.OfType<VictoryPoints>().Sum(e => e.Points);
                    strengthFromWonders += wonder.Effects.OfType<Strength>().Sum(e => e.Points);
                    coinsFromWonders += wonder.Effects.OfType<GetMoney>().Sum(e => e.Money);
                }
                else
                {
                    remainingWonderVP += wonder.Effects.OfType<VictoryPoints>().Sum(e => e.Points);
                    remainingWonderMilitary += wonder.Effects.OfType<Strength>().Sum(e => e.Points);
                    remainingWonderEconomic += wonder.Effects.OfType<GetMoney>().Sum(e => e.Money) / 10f;
                    remainingWonderEconomic += wonder.Effects.OfType<GetMoneyForCard>().Sum(e => e.MoneyPerCard) / 10f;
                    remainingWonderEconomic += wonder.Effects.OfType<GetMoneyForWonders>().Sum(e => e.MoneyPerWonder) / 10f;
                    remainingWonderScience += wonder.Effects.OfType<ChooseDevelopment>().Count();
                    remainingExtraTurn += wonder.Effects.OfType<NewTurn>().Count();
                }
            }


            EconomicAnalysis economics = m_helper.AnalyzeEconomics(playerProperties);
            ScienceAnalysis science = m_helper.AnalyzeScience(playerProperties);
            int wondersBuilt = playerProperties.Owner.Wonders.Count(w => w.HasBeenBuilt);

            properties["Money"] = playerProperties.Owner.Money / 100f;
            properties[nameof(VictoryPoints)] = playerProperties.VictoryPoints / 60f;
            properties[nameof(Strength)] = playerProperties.Strength / 30f;
            properties[$"{nameof(VictoryPoints)}_from_Wonders"] = vpFromWonders / 60f;
            properties[$"{nameof(Strength)}_from_Wonders"] = strengthFromWonders / 30f;
            properties["Coins_from_Wonders"] = coinsFromWonders / 100f;
            properties["RemainingWonderVP_Potential"] = remainingWonderVP / 60f;
            properties["RemainingWonderMilitary_Potential"] = remainingWonderMilitary / 30f;
            properties["RemainingWonderEconomic_Potential"] = remainingWonderEconomic;
            properties["RemainingWonderScience_Potential"] = remainingWonderScience / 4f;
            properties["RemainingExtraTurnPotential"] = remainingExtraTurn / 4f;
            properties["economic_scaling_potential"] = economics.ScalingPotential;
            properties["military_scaling_potential"] = m_helper.CalculateRemainingMilitaryStrength(playerProperties) + remainingWonderMilitary / 30f;
            properties["science_scaling_potential"] = science.Distinct / (float)science.DisciplineCount;
            properties["denial_value"] = economics.DenialValue;
            if (m_game.Context.TurnHandler.NewTurnForced)
            {
                properties["delta_action_budget"] = (m_game.Context.TurnHandler.CurrentPlayer == playerProperties.Owner) ? 1f : -1f;
            }
            else
            {
                properties["delta_action_budget"] = 0f;
            }
            properties["TotalCards"] = playerProperties.Owner.Cards.Count / 35f;
            properties["ScienceCompleteSets"] = science.CompleteSets / 5f;
            properties["ScienceMaxSingle"] = science.MaxSingle / 10f;
            properties["ScienceDistinct"] = science.Distinct / (float)science.DisciplineCount;
            properties["resource_flexibility"] = m_helper.CalculateResourceFlexibility(playerProperties);
            properties["affordable_cards_ratio"] = m_helper.CalculateAffordableCardsRatio(playerProperties);
            properties["WondersBuiltCount"] = wondersBuilt / 4f;
            properties["WondersRemaining"] = (4 - wondersBuilt) / 4f;

            vector.AddRange(properties.Values);
        }

        private static OrderedDictionary<string, float> CreatePlayerProperties()
        {
            var properties = new OrderedDictionary<string, float>();
            properties.Add("Money", 0f);
            properties.Add(nameof(VictoryPoints), 0f);
            properties.Add(nameof(Strength), 0f);
            properties.Add($"{nameof(VictoryPoints)}_from_Wonders", 0f);
            properties.Add($"{nameof(Strength)}_from_Wonders", 0f);
            properties.Add("Coins_from_Wonders", 0f);
            properties.Add("RemainingWonderVP_Potential", 0f);
            properties.Add("RemainingWonderMilitary_Potential", 0f);
            properties.Add("RemainingWonderEconomic_Potential", 0f);
            properties.Add("RemainingWonderScience_Potential", 0f);
            properties.Add("RemainingExtraTurnPotential", 0f);
            properties.Add("economic_scaling_potential", 0f);
            properties.Add("military_scaling_potential", 0f);
            properties.Add("science_scaling_potential", 0f);
            properties.Add("denial_value", 0f);
            properties.Add("delta_action_budget", 0f);
            properties.Add("TotalCards", 0f);
            properties.Add("ScienceCompleteSets", 0f);
            properties.Add("ScienceMaxSingle", 0f);
            properties.Add("ScienceDistinct", 0f);
            properties.Add("resource_flexibility", 0f);
            properties.Add("affordable_cards_ratio", 0f);
            properties.Add("WondersBuiltCount", 0f);
            properties.Add("WondersRemaining", 0f);

            return properties;
        }

        private readonly IHardEncoderHelper m_helper;
        private readonly IGame m_game;
    }
}
