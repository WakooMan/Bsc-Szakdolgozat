using SevenWonders.Game.Logic.Elements.GameCards;
using System.Numerics;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Engine.ChildObjects;

namespace SevenWonders.Game.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class PurpleCardChildTextureHandler: BaseCardChildTextureHandler<PurpleCard>
    {
        public PurpleCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver, ITextureIdHandler textureIdHandler) : base("PurpleCard", gameEngineReceiver, textureIdHandler)
        {
        }

        protected override void HandleCard(PurpleCard card, GameObject gameObject)
        {
            Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
            if (frontSprite is null || frontSprite.Frames.Count == 0)
            {
                return;
            }

            float iconWidthPercent = 0.15f;
            float iconHeightPercent = 0.15f;
            int disciplineTextureId = m_textureIdHandler.GetTextureId(card.GuildObj.GetType().Name);
            ChildTexture childTexture = new ChildTexture
            {
                TextureId = disciplineTextureId,
                WidthPercent = iconWidthPercent,
                HeightPercent = iconHeightPercent,
                PositionPercent = new Vector2(0.425f, 0.2f - iconHeightPercent)
            };
            frontSprite.AddChildObject(childTexture);
        }
    }
}
