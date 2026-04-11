using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace SevenWonders.Common
{
    [ExcludeFromCodeCoverage]
    public class RandomGenerator : IRandomGenerator
    {
        public RandomGenerator() { }
        public int Next()
        {
            return RandomNumberGenerator.GetInt32(int.MaxValue);
        }

        public int Next(int min, int max)
        {
            return RandomNumberGenerator.GetInt32(min, max);
        }
    }
}
