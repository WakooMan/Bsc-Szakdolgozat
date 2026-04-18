namespace SevenWonders.Common
{
    public class RandomGeneratorFactory : IRandomGeneratorFactory
    {
        public IRandomGenerator Create(RandomGeneratorType gameType, int seed)
        {
            switch(gameType)
                {
                case RandomGeneratorType.Undeterministic:
                    return new DefaultRandomGenerator();
                case RandomGeneratorType.Deterministic:
                    return new SeededRandomGenerator(seed);
                default:
                    throw new ArgumentException($"Unsupported game type: {gameType}");
            }
        }
    }
}
