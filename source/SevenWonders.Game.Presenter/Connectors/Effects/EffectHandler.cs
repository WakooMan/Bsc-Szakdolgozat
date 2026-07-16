using SevenWonders.Game.Logic.Elements.Effects;
using SkiaSharp;
using Effect = SevenWonders.Game.Logic.Elements.Effects.Effect;
using SevenWonders.Game.Engine.ChildObjects;

namespace SevenWonders.Game.Presenter.Connectors.Effects
{
    public class EffectHandler : IEffectHandler
    {
        public ICollection<ChildObject> HandleEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {
            if (m_effectHandlers.TryGetValue(effect.GetType(), out Func<Effect, ITextureIdHandler, ICollection<ChildObject>>? handler))
            {
                return handler(effect, textureIdHandler);
            }

            return [];
        }

        private static ICollection<ChildObject> HandleVictoryPointEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {
            if (effect is VictoryPoints victoryPoints)
            {
                int victoryPointsTextureId = textureIdHandler.GetTextureId(nameof(VictoryPoints));
                return new List<ChildObject>
                {
                    new ChildTextLabel
                    {
                        TextLabel = new TextLabel()
                        {
                            BackgroundTextureId = victoryPointsTextureId,
                            Text = ((VictoryPoints)effect).Points.ToString(),
                        TextColor = SKColors.AntiqueWhite,
                        FontSize = 13,
                        Visible = true,
                    },
                    WidthPercent = 0.15f,
                    HeightPercent = 0.15f,
                    }
                };
            }
            return [];
        }

        private static ICollection<ChildObject> HandleChooseGoodEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {

            if (effect is ChooseGood chooseGood)
            {
                List<ChildObject> childObjects = new List<ChildObject>();
                bool isFirst = true;
                chooseGood.GoodFactories.ForEach(goodFactory =>
                {
                    if (!isFirst)
                    {
                        int slashTextureId = textureIdHandler.GetTextureId("Slash");
                        childObjects.Add(new ChildTexture
                        {
                            TextureId = slashTextureId,
                            WidthPercent = 0.05f,
                            HeightPercent = 0.15f,
                        });
                    }
                    else
                    {
                        isFirst = false;
                    }
                    int goodTextureId = textureIdHandler.GetTextureId(goodFactory.CreateGood().GetType().Name);
                    childObjects.Add(new ChildTexture
                    {
                        TextureId = goodTextureId,
                        WidthPercent = 0.15f,
                        HeightPercent = 0.15f,
                    });
                });
                return childObjects;
            }
            return [];
        }

        private static ICollection<ChildObject> HandleBuyGoodsEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {

            if (effect is BuyGoods buyGood)
            {
                List<ChildObject> childObjects = new List<ChildObject>();
                buyGood.BuyGoodItems.ForEach(buyGoodItem =>
                {
                    if (buyGoodItem.MoneyCost > 0)
                    {
                        int coinTextureId = textureIdHandler.GetTextureId("Coin");
                        childObjects.Add(new ChildTextLabel
                        {
                            TextLabel = new TextLabel()
                            {
                                BackgroundTextureId = coinTextureId,
                                Text = buyGoodItem.MoneyCost.ToString(),
                                TextColor = SKColors.Gold,
                                FontSize = 6,
                                Visible = true,
                            },
                            WidthPercent = 0.15f,
                            HeightPercent = 0.15f,
                        });
                    }

                    int goodTextureId = textureIdHandler.GetTextureId(buyGoodItem.GoodType);
                    childObjects.Add(new ChildTexture
                    {
                        TextureId = goodTextureId,
                        WidthPercent = 0.15f,
                        HeightPercent = 0.15f,
                    });
                });
                return childObjects;
            }
            return [];
        }

        private static ICollection<ChildObject> HandleGetMoneyForCardEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {

            if (effect is GetMoneyForCard getMoneyForCard)
            {
                List<ChildObject> childObjects = new List<ChildObject>();
                if (getMoneyForCard.MoneyPerCard > 0)
                {
                    int coinTextureId = textureIdHandler.GetTextureId("Coin");
                    childObjects.Add(new ChildTextLabel
                    {
                        TextLabel = new TextLabel()
                        {
                            BackgroundTextureId = coinTextureId,
                            Text = getMoneyForCard.MoneyPerCard.ToString(),
                            TextColor = SKColors.Gold,
                            FontSize = 6,
                            Visible = true,
                        },
                        WidthPercent = 0.15f,
                        HeightPercent = 0.15f,
                    });
                }

                int cardTextureId = textureIdHandler.GetTextureId(getMoneyForCard.CardType);
                childObjects.Add(new ChildTexture
                {
                    TextureId = cardTextureId,
                    WidthPercent = 0.15f,
                    HeightPercent = 0.15f,
                });
                return childObjects;
            }
            return [];
        }

        private static ICollection<ChildObject> HandleGetMoneyEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {

            if (effect is GetMoney getMoney)
            {
                List<ChildObject> childObjects = new List<ChildObject>();
                if (getMoney.Money > 0)
                {
                    int coinTextureId = textureIdHandler.GetTextureId("Coin");
                    childObjects.Add(new ChildTextLabel
                    {
                        TextLabel = new TextLabel()
                        {
                            BackgroundTextureId = coinTextureId,
                            Text = getMoney.Money.ToString(),
                            TextColor = SKColors.Gold,
                            FontSize = 6,
                            Visible = true,
                        },
                        WidthPercent = 0.15f,
                        HeightPercent = 0.15f,
                    });
                }
                return childObjects;
            }
            return [];
        }

        private static ICollection<ChildObject> HandleGetMoneyForWondersEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {

            if (effect is GetMoneyForWonders getMoneyForWonders)
            {
                List<ChildObject> childObjects = new List<ChildObject>();
                if (getMoneyForWonders.MoneyPerWonder > 0)
                {
                    int coinTextureId = textureIdHandler.GetTextureId("Coin");
                    childObjects.Add(new ChildTextLabel
                    {
                        TextLabel = new TextLabel()
                        {
                            BackgroundTextureId = coinTextureId,
                            Text = getMoneyForWonders.MoneyPerWonder.ToString(),
                            TextColor = SKColors.Gold,
                            FontSize = 6,
                            Visible = true,
                        },
                        WidthPercent = 0.15f,
                        HeightPercent = 0.15f,
                    });

                    childObjects.Add(new ChildTexture
                    {
                        TextureId = textureIdHandler.GetTextureId(nameof(GetMoneyForWonders)),
                        WidthPercent = 0.15f,
                        HeightPercent = 0.15f,
                    });
                }
                return childObjects;
            }
            return [];
        }

        private static ICollection<ChildObject> HandleEnemyLoseMoneyEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {
            if (effect is EnemyLoseMoney enemyLoseMoney)
            {
                if (enemyLoseMoney.Money > 0)
                {
                    int coinTextureId = textureIdHandler.GetTextureId(nameof(EnemyLoseMoney));
                    return [new ChildTextLabel
                    {
                        TextLabel = new TextLabel()
                        {
                            BackgroundTextureId = coinTextureId,
                            Text = enemyLoseMoney.Money.ToString(),
                            TextColor = SKColors.Red,
                            FontSize = 10,
                            Visible = true,
                        },
                        WidthPercent = 0.15f,
                        HeightPercent = 0.15f,
                    }];
                }
            }
            return [];
        }

        private static ICollection<ChildObject> HandleNewTurnEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {
            if (effect is NewTurn newTurn)
            {
                int textureId = textureIdHandler.GetTextureId(nameof(NewTurn));
                return [new ChildTexture
                {
                    TextureId = textureId,
                    WidthPercent = 0.15f,
                    HeightPercent = 0.15f,
                }];
            }
            return [];
        }

        private static ICollection<ChildObject> HandleBuildFreeFromDroppedCardsEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {
            if (effect is BuildFreeFromDroppedCards buildFreeFromDroppedCards)
            {
                int textureId = textureIdHandler.GetTextureId(nameof(BuildFreeFromDroppedCards));
                return [new ChildTexture
                {
                    TextureId = textureId,
                    WidthPercent = 0.15f,
                    HeightPercent = 0.15f,
                }];
            }
            return [];
        }

        private static ICollection<ChildObject> HandleDropEnemyCardEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {
            if (effect is DropEnemyCard dropEnemyCard)
            {
                int textureId = textureIdHandler.GetTextureId(dropEnemyCard.CardType + nameof(DropEnemyCard));
                return [new ChildTexture
                {
                    TextureId = textureId,
                    WidthPercent = 0.15f,
                    HeightPercent = 0.15f,
                }];
            }
            return [];
        }

        private static ICollection<ChildObject> HandleChooseDevelopmentEffect(Effect effect, ITextureIdHandler textureIdHandler)
        {
            if (effect is ChooseDevelopment chooseDevelopment)
            {
                int textureId = textureIdHandler.GetTextureId("developmentbackground");
                return [new ChildTexture
                {
                    TextureId = textureId,
                    WidthPercent = 0.15f,
                    HeightPercent = 0.15f,
                }];
            }
            return [];
        }

        private readonly Dictionary<Type, Func<Effect, ITextureIdHandler, ICollection<ChildObject>>> m_effectHandlers = new Dictionary<Type, Func<Effect, ITextureIdHandler, ICollection<ChildObject>>>()
        {
            { typeof(VictoryPoints), HandleVictoryPointEffect },
            { typeof(ChooseGood), HandleChooseGoodEffect },
            { typeof(BuyGoods), HandleBuyGoodsEffect },
            { typeof(GetMoneyForCard), HandleGetMoneyForCardEffect },
            { typeof(GetMoney), HandleGetMoneyEffect },
            { typeof(GetMoneyForWonders), HandleGetMoneyForWondersEffect },
            { typeof(EnemyLoseMoney), HandleEnemyLoseMoneyEffect },
            { typeof(NewTurn), HandleNewTurnEffect },
            { typeof(BuildFreeFromDroppedCards), HandleBuildFreeFromDroppedCardsEffect },
            { typeof(DropEnemyCard), HandleDropEnemyCardEffect },
            { typeof(ChooseDevelopment), HandleChooseDevelopmentEffect }

        };
    }
}
