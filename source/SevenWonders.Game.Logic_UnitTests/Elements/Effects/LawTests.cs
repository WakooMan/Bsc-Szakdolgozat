using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class LawTests
    {
        [SetUp]
        public void Setup()
        {
            m_law = new Law();
        }

        [Test]
        public void When_Clone_Called()
        {
            Law law = m_law.Clone();

            Assert.That(law, Is.Not.Null);
            Assert.That(m_law, Is.Not.EqualTo(law));
        }

        [Test]
        public void When_OnCalculatePlayerProperties_Called()
        {
            Player player = new Player();
            Player opponent = new Player();
            PlayerProperties playerProperties = new PlayerProperties(player, opponent);

            m_law.OnCalculatePlayerProperties(playerProperties);

            Assert.That(playerProperties.Disciplines.Count, Is.GreaterThan(0));
        }

        private Law m_law;
    }
}
