using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.AI.Model.Services
{
    public interface IRewardCalculator
    {
        float CalculateVictoryPointsReward(PlayerProperties playerProperties, PlayerProperties opponentProperties);
        float CalculateTurnReward(PlayerProperties playerProperties, PlayerProperties opponentProperties);
        float CalculateInstantWinReward(PlayerProperties winner, int playerId);
        void Reset();
    }
}
