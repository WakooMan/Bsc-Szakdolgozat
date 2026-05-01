using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.AI.Model.Services.Encoders.Structs;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class CardEffectAnalyzer : ICardEffectAnalyzer
    {
        public CardEffectAnalysis AnalyzeCardEffects(Card card, PlayerProperties ownerProperties, int moneyCost)
        {
            int deltaVP = 0;
            int deltaStrength = 0;
            int deltaCoins = -moneyCost;
            int deltaResourceCount = 0;
            float futureCostReduction = 0f;
            float denialValue = 0f;
            Type? scienceDiscipline = null;

            switch (card)
            {
                case BlueCard blueCard:
                    deltaVP += blueCard.Point.Points;
                    break;
                case RedCard redCard:
                    deltaStrength += redCard.Strength.Points;
                    break;
                case BrownCard brownCard:
                    deltaResourceCount += brownCard.ProducedResources.Sum(r => r.Amount);
                    break;
                case GrayCard grayCard:
                    deltaResourceCount += grayCard.CreatedProducts.Sum(p => p.Amount);
                    break;
                case GreenCard greenCard:
                    deltaVP += greenCard.Point.Points;
                    scienceDiscipline = greenCard.Discipline.GetType();
                    break;
                case YellowCard yellowCard:
                    AnalyzeYellowCard(yellowCard, ownerProperties, ref deltaVP, ref deltaStrength, ref deltaCoins,
                        ref futureCostReduction, ref denialValue);
                    break;
                case PurpleCard purpleCard:
                    deltaVP += purpleCard.GuildObj.CalculateGuildVP(ownerProperties);
                    deltaCoins += purpleCard.GuildObj.CalculateMoney(ownerProperties);
                    break;
            }

            return new CardEffectAnalysis
            {
                DeltaVP = deltaVP,
                DeltaStrength = deltaStrength,
                DeltaCoins = deltaCoins,
                DeltaResourceCount = deltaResourceCount,
                FutureCostReduction = futureCostReduction,
                DenialValue = denialValue,
                ScienceDiscipline = scienceDiscipline
            };
        }

        private static void AnalyzeYellowCard(YellowCard yellowCard, PlayerProperties ownerProperties,
            ref int deltaVP, ref int deltaStrength, ref int deltaCoins,
            ref float futureCostReduction, ref float denialValue)
        {
            deltaVP = yellowCard.Effects.OfType<VictoryPoints>().Sum(p => p.Points);
            deltaStrength = yellowCard.Effects.OfType<Strength>().Sum(p => p.Points);

            foreach (var effect in yellowCard.Effects)
            {
                switch (effect)
                {
                    case GetMoney getMoney:
                        deltaCoins += getMoney.Money;
                        break;
                    case GetMoneyForCard getMoneyForCard:
                        int cardIncome = getMoneyForCard.GetMoneyForCardValue(ownerProperties.Owner);
                        deltaCoins += cardIncome;
                        break;
                    case GetMoneyForWonders getMoneyForWonders:
                        int wonderIncome = getMoneyForWonders.GetTotalMoney(ownerProperties.Owner);
                        deltaCoins += wonderIncome;
                        break;
                    case CheaperBuilding cheaperBuilding:
                        futureCostReduction += cheaperBuilding.AmountOfResources / 10f;
                        break;
                    case EnemyLoseMoney enemyLoseMoney:
                        denialValue += enemyLoseMoney.Money / 10f;
                        break;
                    case DropEnemyCard:
                        denialValue += 0.5f;
                        break;
                }
            }
        }
    }
}
