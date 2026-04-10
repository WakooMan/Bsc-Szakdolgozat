using SevenWonders.GameEngine;
using Effect = GameLogic.Elements.Effects.Effect;

namespace SevenWonders.Presenter.Connectors.Effects
{
    public interface IEffectHandler
    {
        ICollection<ChildObject> HandleEffect(Effect effect, ITextureIdHandler textureIdHandler);
    }
}
