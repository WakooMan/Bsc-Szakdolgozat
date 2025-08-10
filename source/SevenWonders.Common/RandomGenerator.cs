using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace SevenWonders.Common
{
    [Export(typeof(IRandomGenerator))]
    [ExcludeFromCodeCoverage]
    public class RandomGenerator : IRandomGenerator
    {
        [ImportingConstructor]
        public RandomGenerator() { }
        public int Next()
        {
            return RandomNumberGenerator.GetInt32(int.MaxValue);
        }
    }
}
