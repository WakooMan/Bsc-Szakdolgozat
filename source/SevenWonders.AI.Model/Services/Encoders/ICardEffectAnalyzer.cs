using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using SevenWonders.AI.Model.Services.Encoders.Structs;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public interface ICardEffectAnalyzer
    {
        CardEffectAnalysis AnalyzeCardEffects(Card card, PlayerProperties ownerProperties, int moneyCost);
    }
}
