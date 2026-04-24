using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods.Factories;
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

            if (m_previousPlayer is not null && m_previousOpponent is not null)
            {
                previousPlayerPoints -= (m_previousOpponent.Strength - m_previousPlayer.Strength >= m_strengthDangerThreshold) ? 50f : 0f;
                previousOpponentPoints -= (m_previousPlayer.Strength - m_previousOpponent.Strength >= m_strengthDangerThreshold) ? 50f : 0f;
            }
            playerPoints -= (opponentProperties.Strength - playerProperties.Strength >= m_strengthDangerThreshold) ? 50f : 0f;
            opponentPoints -= (playerProperties.Strength - opponentProperties.Strength >= m_strengthDangerThreshold) ? 50f : 0f;
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

        public float CalculateInstantWinReward(PlayerProperties winner, int playerId)
        {
            float reward = winner.Owner.Id == playerId ? 100f : -100f;
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
            int i = 0;
            foreach(var discipline in playerProperties.Disciplines)
            {
               result += discipline.Value * 1.5f + i * 2.0f;
               i++;
            }
            result += playerProperties.Owner.Money * m_moneyScore;
            foreach (var good in playerProperties.Goods)
            {
                for (int idx = 0; idx < good.Value.Amount; idx++)
                {
                    result += 2.0f - idx * 0.2f;
                }
            }

            result += ScoreEffects(playerProperties, playerProperties.Effects);

            return result;
        }
        private float ScoreEffects(PlayerProperties playerProps, IReadOnlyList<Effect> effects)
        {
            float score = 0f;
            foreach (Effect effect in effects)
            {
                score += effect switch
                {
                    GetMoney getMoney => ScoreMoney(playerProps, getMoney.Money),
                    GetMoneyForCard getMoneyForCard => ScoreGetMoneyForCard(playerProps, getMoneyForCard),
                    GetMoneyForWonders getMoneyForWonders => ScoreGetMoneyForWonders(playerProps, getMoneyForWonders),
                    EnemyLoseMoney enemyLose => ScoreMoney(playerProps, enemyLose.Money),
                    BuildFreeFromDroppedCards buildFreeFromDroppedCards => ScoreBuildFreeFromDroppedCards(playerProps, buildFreeFromDroppedCards),
                    NewTurn => m_newTurnScore,
                    MoneyOnChainBuild moneyOnChainBuild => ScoreMoneyOnChainBuild(playerProps, moneyOnChainBuild),
                    CheaperBuilding cheaperBuilding => ScoreCheaperBuilding(playerProps, cheaperBuilding),
                    BuyGoods buyGoods => ScoreBuyGoods(playerProps, buyGoods),
                    ChooseGood chooseGood => ScoreChooseGood(playerProps, chooseGood),
                    VictoryPoints vp => ScoreVictoryPoints(playerProps, vp),
                    Strength str => ScoreStrength(playerProps, str),
                    ChooseDevelopment => m_chooseDevelopmentScore,
                    DropEnemyCard dropEnemyCard => ScoreDropEnemyCard(playerProps, dropEnemyCard),
                    Mathematics => ScoreVictoryPoints(playerProps, new VictoryPoints() { Points = 3 * playerProps.Owner.Developments.Count }),
                    PlusStrengthOnRedCardBuild plus => ScoreStrength(playerProps, plus.AdditionalStrength) * playerProps.Owner.Cards.OfType<RedCard>().Count(),
                    Law => 6.0f,
                    Economics => m_economicsScore,
                    Teology teology => ScoreTeology(playerProps, teology),
                    _ => 1.0f
                };
            }
            return score;
        }

        private float ScoreTeology(PlayerProperties playerProps, Teology teology)
        {
            return 10.0f;
        }

        private float ScoreDropEnemyCard(PlayerProperties playerProps, DropEnemyCard dropEnemyCard)
        {
            return 5.0f;
        }

        private float ScoreChooseGood(PlayerProperties playerProps, ChooseGood chooseGood)
        {
            float score = 0f;
            foreach (GoodFactory goodFactory in chooseGood.GoodFactories)
            {
                score += Math.Max(0f, m_goodScore / chooseGood.GoodFactories.Count);
            }
            return score;
        }

        private float ScoreBuyGoods(PlayerProperties playerProps, BuyGoods buyGoods)
        {
            float score = 0f;
            foreach (BuyGoodItem buyGoodItem in buyGoods.BuyGoodItems)
            {
                score += Math.Max(0f, m_goodScore - ScoreMoney(playerProps, buyGoodItem.MoneyCost));
            }
            return score;
        }

        private float ScoreCheaperBuilding(PlayerProperties playerProps, CheaperBuilding cheaperBuilding)
        {
            return 5.0f;
        }

        private float ScoreMoneyOnChainBuild(PlayerProperties playerProps, MoneyOnChainBuild moneyOnChainBuild)
        {
            return ScoreMoney(playerProps, moneyOnChainBuild.MoneyToGet.Money * playerProps.Owner.Cards.Count(card => card.HasChainChild));
        }

        private float ScoreBuildFreeFromDroppedCards(PlayerProperties playerProps, BuildFreeFromDroppedCards buildFreeFromDroppedCards)
        {
            return m_buildFreeFromDroppedCardScore;
        }

        private float ScoreGetMoneyForWonders(PlayerProperties playerProps, GetMoneyForWonders getMoneyForWonders)
        {
            return ScoreMoney(playerProps, 4);
        }

        private float ScoreGetMoneyForCard(PlayerProperties playerProps, GetMoneyForCard getMoneyForCard)
        {
            return ScoreMoney(playerProps, 4);
        }

        private float ScoreStrength(PlayerProperties playerProps, Strength strength)
        {
            return strength.Points * m_strengthScore;
        }

        private float ScoreVictoryPoints(PlayerProperties playerProps, VictoryPoints victoryPoints)
        {
            return victoryPoints.Points * m_victoryPointScore;
        }

        private float ScoreMoney(PlayerProperties playerProps, int money)
        {
            return money * m_moneyScore;
        }

        private float m_strengthScore = 2.0f;
        private int m_strengthDangerThreshold = 6;
        private float m_victoryPointScore = 1.5f;
        private float m_moneyScore = 0.3f;
        private float m_goodScore = 3.0f;
        private float m_newTurnScore = 10.0f;
        private float m_chooseDevelopmentScore = 5.0f;
        private float m_economicsScore = 3.0f;
        private float m_buildFreeFromDroppedCardScore = 5.0f;

        private PlayerProperties? m_previousPlayer;
        private PlayerProperties? m_previousOpponent;
    }
}
