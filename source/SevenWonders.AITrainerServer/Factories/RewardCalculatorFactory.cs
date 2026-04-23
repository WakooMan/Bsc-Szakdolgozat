using SevenWonders.AI.Model.Services;

namespace SevenWonders.AITrainerServer.Factories
{
    public class RewardCalculatorFactory : IRewardCalculatorFactory
    {
        public IRewardCalculator Create()
        {
            return new RewardCalculator();
        }
    }
}
