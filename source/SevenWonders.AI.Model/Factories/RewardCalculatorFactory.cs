using SevenWonders.AI.Model.Services;

namespace SevenWonders.AI.Model.Factories
{
    public class RewardCalculatorFactory : IRewardCalculatorFactory
    {
        public IRewardCalculator Create()
        {
            return new RewardCalculator();
        }
    }
}
