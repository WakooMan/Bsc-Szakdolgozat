using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using Microsoft.Maui.Controls;
using SevenWonders.GameEngine;
using SkiaSharp;
using System.Numerics;
using Effect = GameLogic.Elements.Effects.Effect;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class YellowCardChildTextureHandler: BaseCardChildTextureHandler<YellowCard>
    {
        public YellowCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver) : base(TextureIdDictionary.GetTextureId("YellowCardHeader"), gameEngineReceiver)
        {
        }

        protected override void HandleCard(YellowCard card, GameObject gameObject)
        {
            List<ChildObject> childObjects = new List<ChildObject>();
            card.Effects.ForEach(effect =>
            {
                if (m_effectHandlers.TryGetValue(effect.GetType(), out Func<Effect, ICollection<ChildObject>>? handler))
                {
                    childObjects.AddRange(handler(effect));
                }
            });

            Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
            if (frontSprite is not null && frontSprite.Frames.Count > 0)
            {
                float totalWidthPercent = childObjects.Sum(co => co.WidthPercent);
                float groupStartX = (1f - totalWidthPercent) / 2f;
                float centeredY = (0.2f - childObjects.First().HeightPercent);
                float currentX = groupStartX;
                foreach (ChildObject childObject in childObjects)
                {
                    childObject.PositionPercent = new Vector2(currentX, centeredY);
                    frontSprite.AddChildObject(childObject);
                    currentX += childObject.WidthPercent;
                }
            }
        }

        private static ICollection<ChildObject> HandleVictoryPointEffect(Effect effect)
        {
            if (effect is VictoryPoints victoryPoints)
            {
                int victoryPointsTextureId = TextureIdDictionary.GetTextureId(nameof(VictoryPoints));
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

        private static ICollection<ChildObject> HandleChooseGoodEffect(Effect effect)
        {

            if (effect is ChooseGood chooseGood)
            {
                List<ChildObject> childObjects = new List<ChildObject>();
                bool isFirst = true;
                chooseGood.GoodFactories.ForEach(goodFactory =>
                {
                    if (!isFirst)
                    {
                        int slashTextureId = TextureIdDictionary.GetTextureId("Slash");
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
                    int goodTextureId = TextureIdDictionary.GetTextureId(goodFactory.CreateGood().GetType().Name);
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

        private static ICollection<ChildObject> HandleBuyGoodsEffect(Effect effect)
        {

            if (effect is BuyGoods buyGood)
            {
                List<ChildObject> childObjects = new List<ChildObject>();
                buyGood.BuyGoodItems.ForEach(buyGoodItem =>
                {
                    if (buyGoodItem.MoneyCost > 0)
                    {
                        int coinTextureId = TextureIdDictionary.GetTextureId("Coin");
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

                    int goodTextureId = TextureIdDictionary.GetTextureId(buyGoodItem.GoodType);
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

        private static ICollection<ChildObject> HandleGetMoneyForCardEffect(Effect effect)
        {

            if (effect is GetMoneyForCard getMoneyForCard)
            {
                List<ChildObject> childObjects = new List<ChildObject>();
                if (getMoneyForCard.MoneyPerCard > 0)
                {
                    int coinTextureId = TextureIdDictionary.GetTextureId("Coin");
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

                int cardTextureId = TextureIdDictionary.GetTextureId(getMoneyForCard.CardType);
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

        private static ICollection<ChildObject> HandleGetMoneyEffect(Effect effect)
        {

            if (effect is GetMoney getMoney)
            {
                List<ChildObject> childObjects = new List<ChildObject>();
                if (getMoney.Money > 0)
                {
                    int coinTextureId = TextureIdDictionary.GetTextureId("Coin");
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

        private static ICollection<ChildObject> HandleGetMoneyForWondersEffect(Effect effect)
        {

            if (effect is GetMoneyForWonders getMoneyForWonders)
            {
                List<ChildObject> childObjects = new List<ChildObject>();
                if (getMoneyForWonders.MoneyPerWonder > 0)
                {
                    int coinTextureId = TextureIdDictionary.GetTextureId("Coin");
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
                }
                return childObjects;
            }
            return [];
        }

        private readonly Dictionary<Type, Func<Effect, ICollection<ChildObject>>> m_effectHandlers = new Dictionary<Type, Func<Effect, ICollection<ChildObject>>>()
        {
            { typeof(VictoryPoints), HandleVictoryPointEffect },
            { typeof(ChooseGood), HandleChooseGoodEffect },
            { typeof(BuyGoods), HandleBuyGoodsEffect },
            { typeof(GetMoneyForCard), HandleGetMoneyForCardEffect },
            { typeof(GetMoney), HandleGetMoneyEffect },
            { typeof(GetMoneyForWonders), HandleGetMoneyForWondersEffect }

        };
    }
}
