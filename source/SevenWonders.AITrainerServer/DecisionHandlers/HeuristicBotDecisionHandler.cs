using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;
using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;
using GameLogic.Elements.Guilds;
using GameLogic.Elements.Military;
using GameLogic.Elements.Wonders;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.AI.Model;
using System;

namespace SevenWonders.AITrainerServer.DecisionHandlers
{
    public abstract class HeuristicBotDecisionHandler
    {
        public HeuristicBotDecisionHandler(IGame game, IWeightConfiguration weightConfiguration)
        {
            m_game = game;
            m_weightConfiguration = weightConfiguration;
            m_weightCalculations = new List<Func<PlayerProperties, PlayerProperties, ICardComposition, IMilitaryBoard, PickCard, int, float>>()
            {
                ScorePickCard,
                ScoreEnemyUsage,
                ScoreWonders
            };
        }

        protected PlayerActionWrapper HandlePlayerActions(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            if (playerActions.All(w => w.PlayerAction is PickCard))
            {
                m_actionId = -1;
                List<(PlayerActionWrapper wrapper, int action, float score)> scoredActions = new List<(PlayerActionWrapper, int, float)>();
                foreach (PlayerActionWrapper wrapper in playerActions)
                {
                    if (wrapper.CanPerform && wrapper.PlayerAction is PickCard pickCard)
                    {
                        if (player.Wonders.Any(w => new BuildWonder(w).CanPerform(m_game.Context)))
                        {
                            scoredActions.Add((wrapper, BUILDWONDER_ACTION_ID, CalculateDecisionWeight(player, pickCard, BUILDWONDER_ACTION_ID)));
                        }
                        Card card = pickCard.CardNode.CardObj;
                        Player opponent = m_game.Players.First(p => p.Id != player.Id);
                        if ((!string.IsNullOrEmpty(card.PreviousBuilding) && player.Cards.Any(c => c.Name == card.PreviousBuilding)) || 
                            m_game.Context.CostCalculator.CanAfford(card, player, opponent))
                        {
                            scoredActions.Add((wrapper, BUILDCARD_ACTION_ID, CalculateDecisionWeight(player, pickCard, BUILDCARD_ACTION_ID)));
                        }
                        scoredActions.Add((wrapper, SELLCARD_ACTION_ID, CalculateDecisionWeight(player, pickCard, SELLCARD_ACTION_ID)));
                    }
                }

                if (scoredActions.Count > 0)
                {
                    //var bestActions = scoredActions.OrderByDescending(x => x.score).Take(3).ToList();
                    //var bestAction = bestActions[m_game.Context.RandomGenerator.Next(0, bestActions.Count - 1)];
                    var bestAction = scoredActions.OrderByDescending(x => x.score).First();
                    m_actionId = bestAction.action;
                    return bestAction.wrapper;
                }
            }
            if (playerActions.All(w => w.PlayerAction is TurnDecision))
            {
                var wrapper = playerActions.FirstOrDefault(w => w.CanPerform && w.PlayerAction.Id ==  m_actionId);
                if (wrapper is not null)
                {
                    return wrapper;
                }
            }

            return playerActions.First(w => w.CanPerform && w.PlayerAction.Id != UNPICKCARD_ACTION_ID);
        }

        private float CalculateDecisionWeight(Player player, PickCard playerAction, int actionId)
        {
            if (actionId < BUILDCARD_ACTION_ID || actionId > BUILDWONDER_ACTION_ID)
            {
                return 0f;
            }

            float score = 0;
            Player opponent = m_game.Players.First(p => p.Id != player.Id);
            PlayerProperties playerProps = player.GetPlayerProperties(opponent);
            PlayerProperties opponentProps = opponent.GetPlayerProperties(player);
            ICardComposition cardComposition = m_game.Context.AgeHandler.CurrentAge.Composition;
            IMilitaryBoard? militaryBoard = m_game.Context.MilitaryBoard;
            if (militaryBoard is null)
            {
                return score;
            }

            foreach (var weightCalculation in m_weightCalculations)
            {
                score += weightCalculation(playerProps, opponentProps, cardComposition, militaryBoard, playerAction, actionId);
            }
            return score;
        }

        private float ScoreCardMoneyCost(PlayerProperties playerProps, PlayerProperties opponentProps, Card card)
        {
                Player player = playerProps.Owner;
                Player opponent = opponentProps.Owner;

                if (!string.IsNullOrEmpty(card.PreviousBuilding) &&
                    player.Cards.Any(c => c.Name == card.PreviousBuilding))
                {
                    return 0f;
                }

                int cost = m_game.Context.CostCalculator.GetBuildCost(card, player, opponent);
                return ScoreMoney(playerProps, cost);
        }

        private float ScoreWonderMoneyCost(PlayerProperties playerProps, PlayerProperties opponentProps, Wonder wonder)
        {
            Player player = playerProps.Owner;
            Player opponent = opponentProps.Owner;

            int cost = m_game.Context.CostCalculator.GetBuildCost(wonder, player, opponent);
            return ScoreMoney(playerProps, cost);
        }

        private float ScoreWonders(PlayerProperties playerProps, PlayerProperties opponentProps, ICardComposition composition, IMilitaryBoard board, PickCard pickCard, int actionId)
        {
            if (actionId != BUILDWONDER_ACTION_ID)
            {
                return 0f;
            }

            var bestCard = playerProps.Owner.Wonders
                .Select(wonder => (wonder, weight: m_weightConfiguration.ObjectWeights.WonderWeights.FirstOrDefault(w => w.Name == wonder.Name)?.Weight ?? 0))
                .OrderByDescending(pair => pair.weight)
                .FirstOrDefault();

            return bestCard.wonder is not null
                ? ScoreWonder(playerProps, opponentProps, bestCard.wonder)
                : 0f;
        }

        private float ScoreWonder(PlayerProperties playerProps, PlayerProperties opponentProps, Wonder wonder)
        {
            if(wonder.HasBeenBuilt || !playerProps.Owner.Wonders.Contains(wonder))
            {
                return 0f;
            }
            return ScoreEffects(playerProps, opponentProps, wonder.Effects) - ScoreWonderMoneyCost(playerProps, opponentProps, wonder);

        }

        private float ScorePickCard(PlayerProperties playerProps, PlayerProperties opponentProps, ICardComposition composition, IMilitaryBoard board, PickCard pickCard, int actionId)
        {
            if (actionId == BUILDCARD_ACTION_ID)
            {
                return ScoreCard(playerProps, opponentProps, pickCard.CardNode.CardObj) - ScoreCardMoneyCost(playerProps, opponentProps, pickCard.CardNode.CardObj);
            }
            else if (actionId == SELLCARD_ACTION_ID)
            {
                return ScoreMoney(playerProps, 2 + playerProps.Owner.Cards.OfType<YellowCard>().Count());
            }
            return 0f;
        }

        private float ScoreCard(PlayerProperties playerProps, PlayerProperties opponentProps, Card card)
        {
            switch (card)
            {
                case BlueCard blueCard:
                    return ScoreBlueCard(playerProps, opponentProps, blueCard);
                case RedCard redCard:
                    return ScoreRedCard(playerProps, opponentProps, redCard);
                case YellowCard yellowCard:
                    return ScoreYellowCard(playerProps, opponentProps, yellowCard);
                case GreenCard greenCard:
                    return ScoreGreenCard(playerProps, opponentProps, greenCard);
                case PurpleCard purpleCard:
                    return ScorePurpleCard(playerProps, opponentProps, purpleCard);
                case BrownCard brownCard:
                    return ScoreBrownCard(playerProps, opponentProps, brownCard);
                case GrayCard grayCard:
                    return ScoreGrayCard(playerProps, opponentProps, grayCard);
                default:
                    return 0f;
            }
        }

        private float ScoreGrayCard(PlayerProperties playerProps, PlayerProperties opponentProps, GrayCard grayCard)
        {
            float score = 0f;
            foreach (Product product in grayCard.CreatedProducts)
            {
                float playerAmount = 0f;
                if (playerProps.Goods.TryGetValue(product.GetType(), out Good? good))
                {
                    playerAmount = good.Amount;
                }

                score += Math.Max(0f, product.Amount * m_goodScore - playerAmount * m_playerGoodScore);
            }
            return score;
        }

        private float ScoreBrownCard(PlayerProperties playerProps, PlayerProperties opponentProps, BrownCard brownCard)
        {
            float score = 0f;
            foreach (GameResource resource in brownCard.ProducedResources)
            {
                float playerAmount = 0f;
                if (playerProps.Goods.TryGetValue(resource.GetType(), out Good? good))
                {
                    playerAmount = good.Amount;
                }

                score += Math.Max(0f, resource.Amount * m_goodScore - playerAmount * m_playerGoodScore);
            }
            return score;
        }

        private float ScorePurpleCard(PlayerProperties playerProps, PlayerProperties opponentProps, PurpleCard purpleCard)
        {
            Player player = playerProps.Owner;
            Player opponent = opponentProps.Owner;
            Guild guild = purpleCard.GuildObj;

            float estimatedVP = guild switch
            {
                StrategistGuild strategistGuild => ScoreStrategistGuild(playerProps, opponentProps, strategistGuild),
                ScienceGuild scienceGuild => ScoreScienceGuild(playerProps, opponentProps, scienceGuild),
                BuilderGuild builderGuild => ScoreBuilderGuild(playerProps, opponentProps, builderGuild),
                TraderGuild traderGuild => ScoreTraderGuild(playerProps, opponentProps, traderGuild),
                SailorGuild sailorGuild => ScoreSailorGuild(playerProps, opponentProps, sailorGuild),
                MagistrateGuild magistrateGuild => ScoreMagistrateGuild(playerProps, opponentProps, magistrateGuild),
                ExtortionistGuild extortionistGuild => ScoreExtortionistGuild(playerProps, opponentProps, extortionistGuild),
                _ => 3.0f
            };

            return estimatedVP;
        }

        private float ScoreGreenCard(PlayerProperties playerProps, PlayerProperties opponentProps, GreenCard greenCard)
        {
            float score = 0f;
            if (playerProps.Disciplines.TryGetValue(greenCard.Discipline.GetType(), out int opponentDisciplineAmount))
            {
                score += (opponentDisciplineAmount <= 1) ? m_cumulativeDisciplineScore : 0f;
            }
            else
            {
                score += m_disciplineScore;
            }
            score += ScoreVictoryPoints(playerProps, opponentProps, greenCard.Point);
            return score;
        }

        private float ScoreYellowCard(PlayerProperties playerProps, PlayerProperties opponentProps, YellowCard yellowCard)
        {
            return ScoreEffects(playerProps, opponentProps, yellowCard.Effects);
        }

        private float ScoreEffects(PlayerProperties playerProps, PlayerProperties opponentProps, ICollection<Effect> effects)
        {
            float score = 0f;
            foreach (Effect effect in effects)
            {
                score += effect switch
                {
                    GetMoney getMoney => ScoreMoney(playerProps, getMoney.Money),
                    GetMoneyForCard getMoneyForCard => ScoreGetMoneyForCard(playerProps, opponentProps, getMoneyForCard),
                    GetMoneyForWonders getMoneyForWonders => ScoreGetMoneyForWonders(playerProps, opponentProps, getMoneyForWonders),
                    EnemyLoseMoney enemyLose => ScoreMoney(opponentProps, enemyLose.Money),
                    BuildFreeFromDroppedCards buildFreeFromDroppedCards => ScoreBuildFreeFromDroppedCards(playerProps, opponentProps, buildFreeFromDroppedCards),
                    NewTurn => m_newTurnScore,
                    MoneyOnChainBuild moneyOnChainBuild => ScoreMoneyOnChainBuild(playerProps, opponentProps, moneyOnChainBuild),
                    CheaperBuilding cheaperBuilding => ScoreCheaperBuilding(playerProps, opponentProps, cheaperBuilding),
                    BuyGoods buyGoods => ScoreBuyGoods(playerProps, opponentProps, buyGoods),
                    ChooseGood chooseGood => ScoreChooseGood(playerProps, opponentProps, chooseGood),
                    VictoryPoints vp => ScoreVictoryPoints(playerProps, opponentProps, vp),
                    Strength str => ScoreStrength(playerProps, opponentProps, str),
                    ChooseDevelopment => m_chooseDevelopmentScore,
                    DropEnemyCard dropEnemyCard => ScoreDropEnemyCard(playerProps, opponentProps, dropEnemyCard),
                    Mathematics => ScoreVictoryPoints(playerProps, opponentProps, new VictoryPoints() { Points = 3 * playerProps.Owner.Developments.Count }),
                    PlusStrengthOnRedCardBuild plus => ScoreStrength(playerProps, opponentProps, plus.AdditionalStrength) * playerProps.Owner.Cards.OfType<RedCard>().Count(),
                    Law => m_cumulativeDisciplineScore,
                    Economics => m_economicsScore,
                    Teology teology => ScoreTeology(playerProps, opponentProps, teology),
                    _ => 1.0f
                };
            }
            return score;
        }

        private float ScoreTeology(PlayerProperties playerProps, PlayerProperties opponentProps, Teology teology)
        {
            return playerProps.Owner.Wonders.Where(w => !w.HasBeenBuilt).Count() * m_newTurnScore;
        }

        private float ScoreDropEnemyCard(PlayerProperties playerProps, PlayerProperties opponentProps, DropEnemyCard dropEnemyCard)
        {
            var bestCard = playerProps.Opponent.Cards
                .Select(card => (card, weight: m_weightConfiguration.ObjectWeights.CardWeights.FirstOrDefault(w => w.Name == card.Name)?.Weight ?? 0))
                .OrderByDescending(pair => pair.weight)
                .FirstOrDefault();

            return bestCard.card is not null
                ? ScoreCard(playerProps, opponentProps, bestCard.card)
                : 0f;
        }

        private float ScoreChooseGood(PlayerProperties playerProps, PlayerProperties opponentProps, ChooseGood chooseGood)
        {
            float score = 0f;
            foreach (GoodFactory goodFactory in chooseGood.GoodFactories)
            {
                float playerAmount = 0f;
                if (playerProps.Goods.TryGetValue(goodFactory.GoodType, out Good? good))
                {
                    playerAmount = good.Amount;
                }

                score += Math.Max(0f, 1 * m_goodScore - playerAmount * m_playerGoodScore);
            }
            return score;
        }

        private float ScoreBuyGoods(PlayerProperties playerProps, PlayerProperties opponentProps, BuyGoods buyGoods)
        {
            float score = 0f;
            foreach (BuyGoodItem buyGoodItem in buyGoods.BuyGoodItems)
            {
                float playerAmount = 0f;
                Type? type = playerProps.Goods.Keys.FirstOrDefault(t => t.Name == buyGoodItem.GoodType);
                if (type is not null && playerProps.Goods.TryGetValue(type, out Good? good))
                {
                    playerAmount = good.Amount;
                }

                score += Math.Max(0f, m_goodScore - playerAmount * m_playerGoodScore - ScoreMoney(playerProps, buyGoodItem.MoneyCost));
            }
            return score;
        }

        private float ScoreCheaperBuilding(PlayerProperties playerProps, PlayerProperties opponentProps, CheaperBuilding cheaperBuilding)
        {
            return 5.0f;
        }

        private float ScoreMoneyOnChainBuild(PlayerProperties playerProps, PlayerProperties opponentProps, MoneyOnChainBuild moneyOnChainBuild)
        {
            return ScoreMoney(playerProps, moneyOnChainBuild.MoneyToGet.Money * playerProps.Owner.Cards.Count(card => card.HasChainChild));
        }

        private float ScoreBuildFreeFromDroppedCards(PlayerProperties playerProps, PlayerProperties opponentProps, BuildFreeFromDroppedCards buildFreeFromDroppedCards)
        {
            var bestCard = m_game.Context.DroppedCardList.Cards
                .Select(card => (card, weight: m_weightConfiguration.ObjectWeights.CardWeights.FirstOrDefault(w => w.Name == card.Name)?.Weight ?? 0))
                .OrderByDescending(pair => pair.weight)
                .FirstOrDefault();

            return bestCard.card is not null
                ? ScoreCard(playerProps, opponentProps, bestCard.card)
                : 0f;
        }

        private float ScoreGetMoneyForWonders(PlayerProperties playerProps, PlayerProperties opponentProps, GetMoneyForWonders getMoneyForWonders)
        {
            return ScoreMoney(playerProps, playerProps.Owner.Wonders.Count(w => w.HasBeenBuilt));
        }

        private float ScoreGetMoneyForCard(PlayerProperties playerProps, PlayerProperties opponentProps, GetMoneyForCard getMoneyForCard)
        {
            return ScoreMoney(playerProps, playerProps.Owner.Cards.Count(c => c.GetType().Name == getMoneyForCard.CardType));
        }

        private float ScoreRedCard(PlayerProperties playerProps, PlayerProperties opponentProps, RedCard redCard)
        {
            return ScoreStrength(playerProps, opponentProps, redCard.Strength);
        }

        private float ScoreBlueCard(PlayerProperties playerProps, PlayerProperties opponentProps, BlueCard blueCard)
        {
            return ScoreVictoryPoints(playerProps, opponentProps, blueCard.Point);
        }

        private float ScoreStrength(PlayerProperties playerProps, PlayerProperties opponentProps, Strength strength)
        {
            return (playerProps.Strength - opponentProps.Strength >= m_strengthDangerThreshold) ? strength.Points * m_cumulativeStrengthScore : strength.Points * m_strengthScore;
        }

        private float ScoreVictoryPoints(PlayerProperties playerProps, PlayerProperties opponentProps, VictoryPoints victoryPoints)
        {
            return (playerProps.VictoryPoints > m_victoryPointThreshold) ? victoryPoints.Points * m_cumulativeVictoryPointScore : victoryPoints.Points * m_victoryPointScore;
        }

        private float ScoreMoney(PlayerProperties playerProps, int money)
        {
            return playerProps.Owner.Money < 3 ? money * m_cumulativeMoneyScore : money * m_moneyScore;
        }

        private float ScoreSailorGuild(PlayerProperties playerProps, PlayerProperties opponentProps, SailorGuild sailorGuild)
        {
            int count = Math.Max(playerProps.Owner.Cards.Where(c => c is GrayCard || c is BrownCard).Count(), playerProps.Opponent.Cards.Where(c => c is GrayCard || c is BrownCard).Count());
            return ScoreVictoryPoints(playerProps, opponentProps, new VictoryPoints() { Points = count }) + ScoreMoney(playerProps, count);
        }

        private float ScoreMagistrateGuild(PlayerProperties playerProps, PlayerProperties opponentProps, MagistrateGuild magistrateGuild)
        {
            int count = Math.Max(playerProps.Owner.Cards.OfType<BlueCard>().Count(), playerProps.Opponent.Cards.OfType<BlueCard>().Count());
            return ScoreVictoryPoints(playerProps, opponentProps, new VictoryPoints() { Points = count }) + ScoreMoney(playerProps, count);
        }

        private float ScoreTraderGuild(PlayerProperties playerProps, PlayerProperties opponentProps, TraderGuild traderGuild)
        {
            int count = Math.Max(playerProps.Owner.Cards.OfType<YellowCard>().Count(), playerProps.Opponent.Cards.OfType<YellowCard>().Count());
            return ScoreVictoryPoints(playerProps, opponentProps, new VictoryPoints() { Points = count }) + ScoreMoney(playerProps, count);
        }

        private float ScoreBuilderGuild(PlayerProperties playerProps, PlayerProperties opponentProps, BuilderGuild builderGuild)
        {
            int count = Math.Max(playerProps.Owner.Wonders.Where(wonder => wonder.HasBeenBuilt).Count(), playerProps.Opponent.Wonders.Where(wonder => wonder.HasBeenBuilt).Count());
            return ScoreVictoryPoints(playerProps, opponentProps, new VictoryPoints() { Points = count * 2 });
        }

        private float ScoreExtortionistGuild(PlayerProperties playerProps, PlayerProperties opponentProps, ExtortionistGuild extortionistGuild)
        {
            int count = Math.Max(playerProps.Owner.Money, playerProps.Opponent.Money);
            return ScoreVictoryPoints(playerProps, opponentProps, new VictoryPoints() { Points = count % 3 });
        }

        private float ScoreScienceGuild(PlayerProperties playerProps, PlayerProperties opponentProps, ScienceGuild scienceGuild)
        {
            int count = Math.Max(playerProps.Owner.Cards.OfType<GreenCard>().Count(), playerProps.Opponent.Cards.OfType<GreenCard>().Count());
            return ScoreVictoryPoints(playerProps, opponentProps, new VictoryPoints() { Points = count }) + ScoreMoney(playerProps, count);
        }

        private float ScoreStrategistGuild(PlayerProperties playerProps, PlayerProperties opponentProps, StrategistGuild strategistGuild)
        {
            int count = Math.Max(playerProps.Owner.Cards.OfType<RedCard>().Count(), playerProps.Opponent.Cards.OfType<RedCard>().Count());
            return ScoreVictoryPoints(playerProps, opponentProps, new VictoryPoints() { Points = count }) + ScoreMoney(playerProps, count);
        }


        private float ScoreEnemyUsage(PlayerProperties properties1, PlayerProperties properties2, ICardComposition composition, IMilitaryBoard board, PickCard pickCard, int actionId)
        {
            switch (pickCard.CardNode.CardObj)
            {
                case BlueCard blueCard:
                    return ScoreEnemyBlueCard(properties1, properties2, blueCard);
                case RedCard redCard:
                    return ScoreEnemyRedCard(properties1, properties2, redCard);
                case YellowCard yellowCard:
                    return ScoreEnemyYellowCard(properties1, properties2, yellowCard);
                case GreenCard greenCard:
                    return ScoreEnemyGreenCard(properties1, properties2, greenCard);
                case PurpleCard purpleCard:
                    return ScoreEnemyPurpleCard(properties1, properties2, purpleCard);
                case BrownCard brownCard:
                    return ScoreEnemyBrownCard(properties1, properties2, brownCard);
                case GrayCard grayCard:
                    return ScoreEnemyGrayCard(properties1, properties2, grayCard);
                default:
                    return 0f;
            }
        }

        private float ScoreEnemyGrayCard(PlayerProperties playerProps, PlayerProperties opponentProps, GrayCard grayCard)
        {
            return ScoreGrayCard(opponentProps, playerProps, grayCard);
        }

        private float ScoreEnemyBrownCard(PlayerProperties playerProps, PlayerProperties opponentProps, BrownCard brownCard)
        {
            return ScoreBrownCard(opponentProps, playerProps, brownCard);
        }

        private float ScoreEnemyPurpleCard(PlayerProperties playerProps, PlayerProperties opponentProps, PurpleCard purpleCard)
        {
            return ScorePurpleCard(opponentProps, playerProps, purpleCard);
        }

        private float ScoreEnemyGreenCard(PlayerProperties playerProps, PlayerProperties opponentProps, GreenCard greenCard)
        {
            return ScoreGreenCard(opponentProps, playerProps, greenCard);
        }

        private float ScoreEnemyYellowCard(PlayerProperties playerProps, PlayerProperties opponentProps, YellowCard yellowCard)
        {
            return ScoreYellowCard(opponentProps, playerProps, yellowCard);
        }

        private float ScoreEnemyRedCard(PlayerProperties playerProps, PlayerProperties opponentProps, RedCard redCard)
        {
            return ScoreRedCard(opponentProps, playerProps, redCard);
        }

        private float ScoreEnemyBlueCard(PlayerProperties playerProps, PlayerProperties opponentProps, BlueCard blueCard)
        {
            return ScoreBlueCard(opponentProps, playerProps, blueCard);
        }


        protected float m_strengthScore = 2.0f;
        protected float m_cumulativeStrengthScore = 10.0f;
        protected int m_strengthDangerThreshold = 6;

        protected float m_victoryPointScore = 1.5f;
        protected float m_cumulativeVictoryPointScore = 4.0f;
        protected int m_victoryPointThreshold = 20;

        protected float m_moneyScore = 0.5f;
        protected float m_cumulativeMoneyScore = 1.0f;

        protected float m_disciplineScore = 5.0f;
        protected float m_cumulativeDisciplineScore = 10.0f;

        protected float m_goodScore = 3.0f;
        protected float m_playerGoodScore = 2.0f;

        protected float m_newTurnScore = 10.0f;

        protected float m_chooseDevelopmentScore = 5.0f;
        protected float m_economicsScore = 3.0f;

        protected const int BUILDCARD_ACTION_ID = 20;
        protected const int SELLCARD_ACTION_ID = 21;
        protected const int BUILDWONDER_ACTION_ID = 22;
        protected const int UNPICKCARD_ACTION_ID = 13;

        private int m_actionId = -1;
        private readonly IGame m_game;
        private readonly IWeightConfiguration m_weightConfiguration;
        private readonly List<Func<PlayerProperties, PlayerProperties, ICardComposition, IMilitaryBoard, PickCard, int, float>> m_weightCalculations;
    }
}
