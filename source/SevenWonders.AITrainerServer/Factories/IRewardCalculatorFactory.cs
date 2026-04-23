using SevenWonders.AI.Model.Services;

namespace SevenWonders.AITrainerServer.Factories
{
    public interface IRewardCalculatorFactory
    {
        IRewardCalculator Create();
    }
}
