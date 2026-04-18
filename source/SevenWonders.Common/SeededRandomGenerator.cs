using System.Diagnostics.CodeAnalysis;

namespace SevenWonders.Common
{
    [ExcludeFromCodeCoverage]
    public class SeededRandomGenerator : IRandomGenerator
    {
        private readonly Random m_random;

        public SeededRandomGenerator(int seed)
        {
            m_random = new Random(seed);
        }

        public int Next() => m_random.Next();

        public int Next(int min, int max) => m_random.Next(min, max);

        public ICollection<T> TryReceiveRandomElements<T>(ICollection<T> elements, int num)
        {
            ArgumentChecker.CheckNull(elements, nameof(elements));
            ArgumentChecker.CheckPredicateForArgument(() => num < 0, nameof(num));

            List<T> elementList = [.. elements];
            return elementList.OrderBy(n => Next()).Take(num).ToList();
        }

        public ICollection<T> ReceiveRandomElements<T>(ICollection<T> elements, int num)
        {
            ArgumentChecker.CheckNull(elements, nameof(elements));
            ArgumentChecker.CheckPredicateForArgument(() => num < 0, nameof(num));
            ArgumentChecker.CheckPredicateForArgument(() => elements.Count < num, $"The size of {nameof(elements)} collection is lower, than the number of elements to receive.");

            List<T> elementList = [.. elements];
            return elementList.OrderBy(n => Next()).Take(num).ToList();
        }
    }
}