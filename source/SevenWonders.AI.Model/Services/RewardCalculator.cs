using GameLogic.Elements;
using GameLogic.Elements.Effects;
using SevenWonders.Common;

namespace SevenWonders.AI.Model.Services
{
    public class RewardCalculator : IRewardCalculator
    {
        public float CalculateTurnReward(PlayerProperties playerProperties, PlayerProperties opponentProperties)
        {
            float previousPlayerPoints = CalculatePlayerPropertiesPoint(m_previousPlayer);
            float previousOpponentPoints = CalculatePlayerPropertiesPoint(m_previousOpponent);
            float playerPoints = CalculatePlayerPropertiesPoint(playerProperties);
            float opponentPoints = CalculatePlayerPropertiesPoint(opponentProperties);

            m_previousPlayer = playerProperties;
            m_previousOpponent = opponentProperties;
            float reward = (playerPoints - opponentPoints) - (previousPlayerPoints - previousOpponentPoints);
            GameLog.Info($"TurnReward={reward:F3} (Player={playerPoints:F1}, Opponent={opponentPoints:F1}, PrevPlayer={previousPlayerPoints:F1}, PrevOpponent={previousOpponentPoints:F1})");
            return reward;
        }

        public float CalculateVictoryPointsReward(PlayerProperties playerProperties, PlayerProperties opponentProperties)
        {
            float reward = playerProperties.VictoryPoints - opponentProperties.VictoryPoints;
            GameLog.Info($"VictoryPointsReward={reward} (PlayerVP={playerProperties.VictoryPoints}, OpponentVP={opponentProperties.VictoryPoints})");
            return reward;
        }

        public float CalculateInstantWinReward(PlayerProperties winner)
        {
            float reward = winner.Owner.Id == 1 ? 100f : -100f;
            GameLog.Info($"InstantWinReward={reward} (Winner={winner.Owner.Name}, Id={winner.Owner.Id})");
            return reward;
        }

        public void Reset()
        {
            GameLog.Info("Reset.");
            m_previousPlayer = null;
            m_previousOpponent = null;
        }

        private float CalculatePlayerPropertiesPoint(PlayerProperties? playerProperties)
        {
            float result = 0f;
            if (playerProperties is null)
                return result;
            result += playerProperties.VictoryPoints * 1.0f;
            result += playerProperties.Strength * 2.0f;
            int i = 0;
            foreach(var discipline in playerProperties.Disciplines)
            {
               result += discipline.Value * 1.5f + i * 2.0f;
               i++;
            }
            result += playerProperties.Owner.Money * 0.2f;
            foreach (var good in playerProperties.Goods)
            {
                result += good.Value.Amount * 0.5f;
            }
            foreach (var effect in playerProperties.Effects)
            {
                if(m_effectRewards.TryGetValue(effect.GetType(), out float reward))
                {
                    result += reward;
                }
            }

            return result;
        }

        private PlayerProperties? m_previousPlayer;
        private PlayerProperties? m_previousOpponent;
        private readonly IDictionary<Type, float> m_effectRewards = new Dictionary<Type, float>
        {
            { typeof(NewTurn), 5.0f },
            { typeof(GetMoneyForCard), 2.0f },
            { typeof(GetMoneyForWonders), 2.0f },
            { typeof(GetMoney), 1.0f },
            { typeof(EnemyLoseMoney), 1.5f },
            { typeof(BuildFreeFromDroppedCards), 4.0f },
            { typeof(ChooseDevelopment), 3.0f },
            { typeof(DropEnemyCard), 3.5f },
            { typeof(Mathematics), 3.0f },
            { typeof(MoneyOnChainBuild), 1.5f },
            { typeof(PlusStrengthOnRedCardBuild), 2.0f },
            { typeof(CheaperBuilding), 2.0f },
            { typeof(Law), 3.0f },
            { typeof(Economics), 2.5f },
            { typeof(Teology), 3.5f },
            { typeof(BuyGoods), 1.5f },
            { typeof(ChooseGood), 1.0f },
        };
    }
}
