using GameLogic.Elements;
using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;
using GameLogic.Elements.Wonders;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class HardPlayerEncoder : IHardPlayerEncoder
    {
        public HardPlayerEncoder()
        {
        }

        public void EncodePlayer(List<float> vector, PlayerProperties playerProperties)
        {
            var properties = CreatePlayerProperties();

            properties["Money"] = playerProperties.Owner.Money / 100f;
            properties[nameof(VictoryPoints)] = playerProperties.VictoryPoints / 60f;
            properties[nameof(Strength)] = playerProperties.Strength / 30f;

            for (int i = 0; i < 4; i++)
            {
                var wonders = playerProperties.Owner.Wonders;
                properties[$"Wonder{i}Built"] = wonders[i].HasBeenBuilt ? 1f : 0f;
                properties[$"Wonder{i}EffectCount"] = wonders[i].Effects.Count / 5f;
                properties[$"Wonder{i}CostCount"] = wonders[i].GoodCost.Count / 5f;
            }

            Type[] goodTypes = [typeof(Clay), typeof(Stone), typeof(Wood), typeof(Glass), typeof(Papirus)];
            foreach (var goodType in goodTypes)
            {
                properties[goodType.Name] = playerProperties.Goods.TryGetValue(goodType, out var good) ? good.Amount / 10f : 0f;
            }

            Type[] disciplineTypes = [typeof(Building), typeof(Geography), typeof(Healing), typeof(Mechanics), typeof(Physics), typeof(Trading), typeof(Writing)];
            int minDiscipline = int.MaxValue;
            int maxDiscipline = 0;
            int totalDisciplines = 0;
            int distinctDisciplines = 0;
            foreach (var disciplineType in disciplineTypes)
            {
                int count = playerProperties.Disciplines.TryGetValue(disciplineType, out var c) ? c : 0;
                properties[disciplineType.Name] = count / 10f;
                if (count > 0)
                {
                    distinctDisciplines++;
                    minDiscipline = Math.Min(minDiscipline, count);
                }
                maxDiscipline = Math.Max(maxDiscipline, count);
                totalDisciplines += count;
            }
            if (minDiscipline == int.MaxValue) minDiscipline = 0;

            properties["ScienceCompleteSets"] = (distinctDisciplines == disciplineTypes.Length ? minDiscipline : 0) / 5f;
            properties["ScienceMaxSingle"] = maxDiscipline / 10f;
            properties["ScienceDistinct"] = distinctDisciplines / (float)disciplineTypes.Length;
            properties["ScienceTotal"] = totalDisciplines / 30f;

            Type[] cardTypes = [typeof(BrownCard), typeof(BlueCard), typeof(GrayCard), typeof(GreenCard), typeof(PurpleCard), typeof(RedCard), typeof(YellowCard)];
            int totalCards = 0;
            foreach (var cardType in cardTypes)
            {
                int count = playerProperties.Owner.Cards.Count(c => c.GetType() == cardType);
                properties[cardType.Name] = count / 10f;
                totalCards += count;
            }
            properties["TotalCards"] = totalCards / 30f;

            int wondersBuilt = playerProperties.Owner.Wonders.Count(w => w.HasBeenBuilt);
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
            properties.Add("tempo_gain", 0f);
            properties.Add("delta_action_budget", 0f);
            properties.Add("future_income_rate", 0f);
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
    }
}
