using SevenWonders.AI.Model.Services;

namespace SevenWonders.AI.Model.Factories
{
    public interface IRewardCalculatorFactory
    {
        IRewardCalculator Create();
    }
}
