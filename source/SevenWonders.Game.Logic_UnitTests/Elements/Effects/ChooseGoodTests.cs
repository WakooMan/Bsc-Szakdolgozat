using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class ChooseGoodTests
    {
        [SetUp]
        public void Setup()
        {
            m_chooseGood = new ChooseGood();
        }

        [Test]
        public void When_Clone_Called()
        {
            ChooseGood chooseGood = m_chooseGood.Clone();

            Assert.That(chooseGood, Is.Not.Null);
            Assert.That(m_chooseGood, Is.Not.EqualTo(chooseGood));
        }

        private ChooseGood m_chooseGood;
    }
}
