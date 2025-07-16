namespace SevenWonders.Common
{
    public class RandomGenerator : IRandomGenerator
    {
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
