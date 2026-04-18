namespace SevenWonders.Common
{
    public interface IRandomGeneratorFactory
    {
        IRandomGenerator Create(RandomGeneratorType gameType, int seed);
    }
}
