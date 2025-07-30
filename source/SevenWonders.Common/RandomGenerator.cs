using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;

namespace SevenWonders.Common
{
    [Export(typeof(IRandomGenerator))]
    [ExcludeFromCodeCoverage]
    public class RandomGenerator : IRandomGenerator
    {
        [ImportingConstructor]
        public RandomGenerator()
        {
            m_random = new Random();

        }
        public int Next()
        {
            return m_random.Next();
        }

        private Random m_random;
    }
}
