using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.Handlers
{
    [Export(typeof(IRandomElementReceiver))]
    public class RandomElementReceiver : IRandomElementReceiver
    {
        [ImportingConstructor]
        public RandomElementReceiver(IRandomGenerator randomGenerator)
        {
            ArgumentChecker.CheckNull(randomGenerator, nameof(randomGenerator));

            m_randomGenerator = randomGenerator;
        }
        public ICollection<T> TryReceiveRandomElements<T>(ICollection<T> elements, int num)
        {
            ArgumentChecker.CheckNull(elements, nameof(elements));
            ArgumentChecker.CheckPredicateForArgument(() => num < 0, nameof(num));

            List<T> elementList = [.. elements];
            return elementList.OrderBy(n => Guid.NewGuid()).Take(num).ToList();
        }

        public ICollection<T> ReceiveRandomElements<T>(ICollection<T> elements, int num)
        {
            ArgumentChecker.CheckNull(elements, nameof(elements));
            ArgumentChecker.CheckPredicateForArgument(() => num < 0, nameof(num));
            ArgumentChecker.CheckPredicateForArgument(() => elements.Count < num, $"The size of {nameof(elements)} collection is lower, than the number of elements to receive.");
            
            List<T> elementList = [.. elements];
            return elementList.OrderBy(n => m_randomGenerator.Next()).Take(num).ToList();
        }

        private readonly IRandomGenerator m_randomGenerator;
    }
}
