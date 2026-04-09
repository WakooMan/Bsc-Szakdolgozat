using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;
using SkiaSharp;
using System.Numerics;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public abstract class BaseCardChildTextureHandler<TCard> : ICardChildTextureHandler where TCard : Card
    {
        protected BaseCardChildTextureHandler(string textureName, IGameEngineReceiver gameEngineReceiver, ITextureIdHandler textureIdHandler)
        {
            m_textureName = textureName;
            m_gameEngineReceiver = gameEngineReceiver;
            m_textureIdHandler = textureIdHandler;
        }

        public void Handle(Card card)
        {
            if (card is TCard typedCard)
            {
                GameObject gameObject = m_gameEngineReceiver.ReceiveGameObject(card.Name);
                Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
                if (frontSprite is null || frontSprite.Frames.Count == 0)
                {
                    return;
                }

                ChildTexture childTexture = new ChildTexture
                {
                    TextureId = m_textureIdHandler.GetTextureId(m_textureName),
                    WidthPercent = 1.0f,
                    HeightPercent = 0.20f,
                    PositionPercent = new Vector2(0f, 0f)
                };

                frontSprite.AddChildObject(childTexture);
                int i = 0;
                float yOffset = 0.22f;
                if (card.GoodCost.Any())
                {
                    card.GoodCost.ForEach(good =>
                    {
                        float sizePercent = 0.15f;
                        ChildTexture goodTexture = new ChildTexture
                        {
                            TextureId = m_textureIdHandler.GetTextureId(good.GetType().Name),
                            WidthPercent = sizePercent,
                            HeightPercent = sizePercent,
                            PositionPercent = new Vector2(0f + i * sizePercent, yOffset)
                        };
                        frontSprite.AddChildObject(goodTexture);
                        i++;
                    });
                    yOffset += 0.15f;
                }

                if (card.MoneyCost > 0)
                {
                    float sizePercent = 0.15f;
                    ChildTextLabel textLabel = new ChildTextLabel
                    {
                        TextLabel = new TextLabel()
                        {
                            Visible = true,
                            Text = card.MoneyCost.ToString(),
                            TextColor = SKColors.Gold,
                            FontSize = 6,
                            BackgroundTextureId = m_textureIdHandler.GetTextureId("Coin")
                        },
                        WidthPercent = sizePercent,
                        HeightPercent = sizePercent,
                        PositionPercent = new Vector2(0f, yOffset)
                    };
                    frontSprite.AddChildObject(textLabel);
                }

                ChildTextLabel childTextLabel = new ChildTextLabel
                {
                    TextLabel = new TextLabel
                    {
                        Text = card.Name,
                        TextColor = SKColors.Wheat,
                        FontSize = 8,
                        BackgroundTextureId = m_textureIdHandler.GetTextureId("CardNameBackground"),
                        Visible = true
                    },
                    WidthPercent = 0.6f,
                    HeightPercent = 0.1f,
                    PositionPercent = new Vector2(0.2f, 0.8f)
                };

                frontSprite.AddChildObject(childTextLabel);
                HandleCard(typedCard, gameObject);
            }
        }

        protected abstract void HandleCard(TCard card, GameObject gameObject);

        private readonly string m_textureName;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        protected readonly ITextureIdHandler m_textureIdHandler;
    }
}
