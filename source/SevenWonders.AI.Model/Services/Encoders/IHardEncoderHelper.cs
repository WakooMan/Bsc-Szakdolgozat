using GameLogic.Elements;
using SevenWonders.AI.Model.Services.Encoders.Structs;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public interface IHardEncoderHelper
    {
        ScienceAnalysis AnalyzeScience(PlayerProperties playerProperties);
        ScienceAnalysis AnalyzeScienceWithAdded(PlayerProperties playerProperties, Type addedDiscipline);
        MilitaryAnalysis AnalyzeMilitary(PlayerProperties ownerProperties, PlayerProperties opponentProperties);
        EconomicAnalysis AnalyzeEconomics(PlayerProperties playerProperties);
        float CalculateAffordableCardsRatio(PlayerProperties ownerProperties);
        float CalculateResourceFlexibility(PlayerProperties playerProperties);
        float CalculateRemainingMilitaryStrength(PlayerProperties playerProperties);
    }
}
