using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Disciplines;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.AI.Model.Services.Encoders.Structs;
using System.Xml.Linq;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class HardEncoderHelper : IHardEncoderHelper
    {
        private static readonly Type[] s_disciplineTypes =
        [
            typeof(Building),
            typeof(Geography),
            typeof(Healing),
            typeof(Mechanics),
            typeof(Physics),
            typeof(Trading),
            typeof(Writing)
        ];

        public HardEncoderHelper(IGame game)
        {
            m_game = game;
        }

        public ScienceAnalysis AnalyzeScience(PlayerProperties playerProperties)
        {
            return AnalyzeScienceInternal(playerProperties, null);
        }

        public ScienceAnalysis AnalyzeScienceWithAdded(PlayerProperties playerProperties, Type addedDiscipline)
        {
            return AnalyzeScienceInternal(playerProperties, addedDiscipline);
        }

        private static ScienceAnalysis AnalyzeScienceInternal(PlayerProperties playerProperties, Type? addedDiscipline)
        {
            int minDiscipline = int.MaxValue;
            int maxDiscipline = 0;
            int totalDisciplines = 0;
            int distinctDisciplines = 0;

            foreach (var disciplineType in s_disciplineTypes)
            {
                int count = playerProperties.Disciplines.TryGetValue(disciplineType, out var c) ? c : 0;
                if (addedDiscipline is not null && disciplineType == addedDiscipline)
                    count++;
                if (count > 0)
                {
                    distinctDisciplines++;
                    minDiscipline = Math.Min(minDiscipline, count);
                }
                maxDiscipline = Math.Max(maxDiscipline, count);
                totalDisciplines += count;
            }
            if (minDiscipline == int.MaxValue) minDiscipline = 0;

            return new ScienceAnalysis
            {
                CompleteSets = distinctDisciplines == s_disciplineTypes.Length ? minDiscipline : 0,
                MaxSingle = maxDiscipline,
                Distinct = distinctDisciplines,
                Total = totalDisciplines,
                DisciplineCount = s_disciplineTypes.Length
            };
        }

        public MilitaryAnalysis AnalyzeMilitary(PlayerProperties ownerProperties, PlayerProperties opponentProperties)
        {
            var militaryBoard = m_game.Context.MilitaryBoard;
            if (militaryBoard is null || militaryBoard.Fields.Count == 0)
            {
                return new MilitaryAnalysis
                {
                    ShieldPosition = 0f,
                    WinProximity = 0.5f,
                    StrengthDiff = ownerProperties.Strength - opponentProperties.Strength,
                    BoardMiddle = 0,
                    BoardLength = 0
                };
            }

            int middle = militaryBoard.Fields.Count / 2;
            int strengthDiff = ownerProperties.Strength - opponentProperties.Strength;
            int currentPos = Math.Clamp(middle + strengthDiff, 0, militaryBoard.Fields.Count - 1);


            return new MilitaryAnalysis
            {
                ShieldPosition = (currentPos - middle) / (float)middle,
                WinProximity = currentPos / (float)(militaryBoard.Fields.Count - 1),
                StrengthDiff = strengthDiff,
                BoardMiddle = middle,
                BoardLength = militaryBoard.Fields.Count
            };
        }

        public EconomicAnalysis AnalyzeEconomics(PlayerProperties playerProperties)
        {
            float scalingPotential = 0f;
            float futureCostReduction = 0f;
            float denialValue = 0f;

            foreach (var effect in playerProperties.Effects)
            {
                switch (effect)
                {
                    case GetMoneyForCard getMoneyForCard:
                        scalingPotential += getMoneyForCard.GetMoneyForCardValue(playerProperties.Owner) / 10f;
                        break;
                    case GetMoneyForWonders getMoneyForWonders:
                        scalingPotential += getMoneyForWonders.GetTotalMoney(playerProperties.Owner) / 10f;
                        break;
                    case CheaperBuilding cheaperBuilding:
                        int matchingBuildables = CountBuildablesOfType(playerProperties.Owner, cheaperBuilding.BuildingType);
                        futureCostReduction += (cheaperBuilding.AmountOfResources * matchingBuildables) / 10f;
                        break;
                    case EnemyLoseMoney enemyLoseMoney:
                        denialValue += enemyLoseMoney.Money / 10f;
                        scalingPotential += enemyLoseMoney.Money / 10f;
                        break;
                    case MoneyOnChainBuild moneyOnChainBuild:
                        int chainBuildOpportunities = CalculateChainBuildOpportunities(playerProperties.Owner);
                        scalingPotential += (moneyOnChainBuild.MoneyToGet.Money * chainBuildOpportunities) / 10f;
                        break;
                }
            }

            return new EconomicAnalysis
            {
                ScalingPotential = scalingPotential,
                FutureCostReduction = futureCostReduction,
                DenialValue = denialValue
            };
        }

        private int CountBuildablesOfType(Player owner, string buildingType)
        {
            int count = 0;

            if (buildingType == nameof(Wonder))
            {
                count += owner.Wonders.Count(w => !w.HasBeenBuilt);
            }

            ICardComposition composition = m_game.Context.AgeHandler.CurrentAge.Composition;
            foreach (ICardNode node in composition.AllCards)
            {
                if (node.CardObj.BuildingType == buildingType &&
                    !owner.Cards.Any(c => c.Name == node.CardObj.Name))
                {
                    count++;
                }
            }

            ICardList? cardList = m_game.Context.CardList;
            if (cardList is not null)
            {
                foreach (Card card in cardList.Cards)
                {
                    if (card.BuildingType == buildingType &&
                        !owner.Cards.Any(c => c.Name == card.Name))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CalculateChainBuildOpportunities(Player owner)
        {
            HashSet<string> ownedCardNames = [];
            foreach (Card card in owner.Cards)
            {
                if (card.HasChainChild)
                {
                    ownedCardNames.Add(card.Name);
                }
            }

            if (ownedCardNames.Count == 0)
                return 0;

            int opportunities = 0;

            ICardComposition composition = m_game.Context.AgeHandler.CurrentAge.Composition;
            foreach (ICardNode node in composition.AllCards)
            {
                if (!string.IsNullOrEmpty(node.CardObj.PreviousBuilding) &&
                    ownedCardNames.Contains(node.CardObj.PreviousBuilding))
                {
                    opportunities++;
                }
            }

            ICardList? cardList = m_game.Context.CardList;
            if (cardList is not null)
            {
                foreach (Card card in cardList.Cards)
                {
                    if (!string.IsNullOrEmpty(card.PreviousBuilding) &&
                        ownedCardNames.Contains(card.PreviousBuilding))
                    {
                        opportunities++;
                    }
                }
            }

            return opportunities;
        }

        public float CalculateAffordableCardsRatio(PlayerProperties ownerProperties)
        {
            ICardComposition composition = m_game.Context.AgeHandler.CurrentAge.Composition;
            int affordable = 0;
            int total = 0;
            foreach (ICardNode node in composition.AvailableCards)
            {
                if (node.Hidden) continue;
                total++;
                if (m_game.Context.CostCalculator.CanAfford(node.CardObj, ownerProperties.Owner, ownerProperties.Opponent))
                    affordable++;
            }
            return total > 0 ? affordable / (float)total : 0f;
        }

        public float CalculateResourceFlexibility(PlayerProperties playerProperties)
        {
            float totalResources = 0f;
            foreach (var good in playerProperties.Goods.Values)
            {
                totalResources += good.Amount;
            }
            return totalResources / 20f;
        }

        public float CalculateRemainingMilitaryStrength(PlayerProperties playerProperties)
        {
            int remainingRedStrength = 0;

            PlusStrengthOnRedCardBuild? plusStrengthOnRedCardBuild = playerProperties.GetEffects<PlusStrengthOnRedCardBuild>().FirstOrDefault();

            ICardComposition composition = m_game.Context.AgeHandler.CurrentAge.Composition;
            foreach (ICardNode node in composition.AllCards)
            {
                if (!playerProperties.Owner.Cards.Contains(node.CardObj) && !playerProperties.Opponent.Cards.Contains(node.CardObj) && node.CardObj is RedCard redCard)
                {
                    if (plusStrengthOnRedCardBuild is not null)
                    {
                        remainingRedStrength += plusStrengthOnRedCardBuild.AdditionalStrength.Points;
                    }
                    remainingRedStrength+= redCard.Strength.Points;
                }
            }

            ICardList? cardList = m_game.Context.CardList;
            if (cardList is not null)
            {
                foreach (Card card in cardList.Cards)
                {
                    if (!playerProperties.Owner.Cards.Contains(card) && !playerProperties.Opponent.Cards.Contains(card) &&card is RedCard redCard)
                    {
                        if (plusStrengthOnRedCardBuild is not null)
                        {
                            remainingRedStrength += plusStrengthOnRedCardBuild.AdditionalStrength.Points;
                        }
                        remainingRedStrength+= redCard.Strength.Points;
                    }
                }
            }



            return remainingRedStrength;
        }

        private readonly IGame m_game;
    }
}
