using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace SevenWonders.Common
{
    [ExcludeFromCodeCoverage]
    public class DefaultRandomGenerator : IRandomGenerator
    {
        public DefaultRandomGenerator() { }
        public int Next()
        {
            return RandomNumberGenerator.GetInt32(int.MaxValue);
        }

        public int Next(int min, int max)
        {
            return RandomNumberGenerator.GetInt32(min, max);
        }

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
