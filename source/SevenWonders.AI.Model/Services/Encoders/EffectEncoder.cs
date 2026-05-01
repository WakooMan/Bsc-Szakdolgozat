using SevenWonders.Game.Logic.Elements.Effects;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class EffectEncoder: IEffectEncoder
    {
        public void EncodeEffect(Effect effect, IDictionary<string, float> cardNodeProperties)
        {
            switch (effect)
            {
                case GetMoney getMoney:
                    cardNodeProperties[nameof(GetMoney)] = getMoney.Money / 10f;
                    break;
                case GetMoneyForCard getMoneyForCard:
                    cardNodeProperties[nameof(GetMoneyForCard) + getMoneyForCard.CardType] = getMoneyForCard.MoneyPerCard / 10f;
                    break;
                case GetMoneyForWonders getMoneyForWonders:
                    cardNodeProperties[nameof(GetMoneyForWonders)] = getMoneyForWonders.MoneyPerWonder / 10f;
                    break;
                case EnemyLoseMoney enemyLoseMoney:
                    cardNodeProperties[nameof(EnemyLoseMoney)] = enemyLoseMoney.Money / 10f;
                    break;
                case VictoryPoints victoryPoints:
                    cardNodeProperties[nameof(VictoryPoints)] = victoryPoints.Points / 100f;
                    break;
                case Strength strength:
                    cardNodeProperties[nameof(Strength)] = strength.Points / 100f;
                    break;
                case MoneyOnChainBuild moneyOnChainBuild:
                    cardNodeProperties[nameof(MoneyOnChainBuild)] = moneyOnChainBuild.MoneyToGet.Money / 10f;
                    break;
                case PlusStrengthOnRedCardBuild plusStrength:
                    cardNodeProperties[nameof(PlusStrengthOnRedCardBuild)] = plusStrength.AdditionalStrength.Points / 100f;
                    break;
                case CheaperBuilding cheaperBuilding:
                    cardNodeProperties[nameof(CheaperBuilding) + cheaperBuilding.BuildingType] = cheaperBuilding.AmountOfResources / 10f;
                    break;
                default:
                    cardNodeProperties[effect.GetType().Name] = 1f;
                    break;
            }
        }
    }
}
