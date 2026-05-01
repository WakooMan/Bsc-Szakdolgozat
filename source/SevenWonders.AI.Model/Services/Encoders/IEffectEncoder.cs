using SevenWonders.Game.Logic.Elements.Effects;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public interface IEffectEncoder
    {
        void EncodeEffect(Effect effect, IDictionary<string, float> cardNodeProperties);
    }
}
