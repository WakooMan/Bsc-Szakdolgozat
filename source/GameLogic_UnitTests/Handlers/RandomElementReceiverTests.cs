using GameLogic.Handlers;
using NSubstitute;
using SevenWonders.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Handlers
{
    public class RandomElementReceiverTests
    {
        [SetUp]
        public void Setup()
        {
            m_randomGenerator = Substitute.For<RandomGenerator>();

            m_randomElementReceiver = new RandomElementReceiver(m_randomGenerator);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new RandomElementReceiver(null));
        }

        [Test]
        public void When_TryReceiveRandomElements_Called_With_Lower_Collection_Size()
        {
            ICollection<int> collection = null;
            Assert.DoesNotThrow(() => collection = m_randomElementReceiver.TryReceiveRandomElements([1, 2], 3));

            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Count, Is.EqualTo(2));
        }

        [Test]
        public void When_TryReceiveRandomElements_Called_With_Higher_Collection_Size()
        {
            ICollection<int> collection = null;
            Assert.DoesNotThrow(() => collection = m_randomElementReceiver.TryReceiveRandomElements([1, 2, 3 , 4], 3));

            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Count, Is.EqualTo(3));
        }

        [Test]
        public void When_TryReceiveRandomElements_Called_With_Null_Collection()
        {
            ICollection<int> collection = null;
            Assert.Throws<ArgumentNullException>(() => collection = m_randomElementReceiver.TryReceiveRandomElements((ICollection<int>)null, 3));
        }

        [Test]
        public void When_TryReceiveRandomElements_Called_With_Negative_Number()
        {
            ICollection<int> collection = null;
            Assert.Throws<ArgumentException>(() => collection = m_randomElementReceiver.TryReceiveRandomElements([1, 2, 3, 4], -3));
        }

        [Test]
        public void When_TryReceiveRandomElements_Called_With_Zero_Number()
        {
            ICollection<int> collection = null;
            Assert.DoesNotThrow(() => collection = m_randomElementReceiver.TryReceiveRandomElements([1, 2, 3, 4], 0));

            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Count, Is.EqualTo(0));
        }

        [Test]
        public void When_ReceiveRandomElements_Called_With_Lower_Collection_Size()
        {
            ICollection<int> collection = null;
            Assert.Throws<ArgumentException>(() => collection = m_randomElementReceiver.ReceiveRandomElements([1, 2], 3));
        }

        [Test]
        public void When_ReceiveRandomElements_Called_With_Higher_Collection_Size()
        {
            ICollection<int> collection = null;
            Assert.DoesNotThrow(() => collection = m_randomElementReceiver.ReceiveRandomElements([1, 2, 3, 4], 3));

            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Count, Is.EqualTo(3));
        }

        [Test]
        public void When_ReceiveRandomElements_Called_With_Null_Collection()
        {
            ICollection<int> collection = null;
            Assert.Throws<ArgumentNullException>(() => collection = m_randomElementReceiver.ReceiveRandomElements((ICollection<int>)null, 3));
        }

        [Test]
        public void When_ReceiveRandomElements_Called_With_Negative_Number()
        {
            ICollection<int> collection = null;
            Assert.Throws<ArgumentException>(() => collection = m_randomElementReceiver.ReceiveRandomElements([1, 2, 3, 4], -3));
        }

        [Test]
        public void When_ReceiveRandomElements_Called_With_Zero_Number()
        {
            ICollection<int> collection = null;
            Assert.DoesNotThrow(() => collection = m_randomElementReceiver.ReceiveRandomElements([1, 2, 3, 4], 0));

            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Count, Is.EqualTo(0));
        }

        private RandomElementReceiver m_randomElementReceiver;
        private IRandomGenerator m_randomGenerator;
    }
}
