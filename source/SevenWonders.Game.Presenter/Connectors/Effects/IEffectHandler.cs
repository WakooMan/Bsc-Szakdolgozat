using SevenWonders.Game.Engine;
using Effect = SevenWonders.Game.Logic.Elements.Effects.Effect;

namespace SevenWonders.Game.Presenter.Connectors.Effects
{
    public interface IEffectHandler
    {
        ICollection<ChildObject> HandleEffect(Effect effect, ITextureIdHandler textureIdHandler);
    }
}
