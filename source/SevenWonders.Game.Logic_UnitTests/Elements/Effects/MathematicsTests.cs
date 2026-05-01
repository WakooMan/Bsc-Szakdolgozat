using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.Modifiers;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class MathematicsTests
    {
        [SetUp]
        public void Setup()
        {
            m_mathematics = new Mathematics();
        }

        [Test]
        public void When_Clone_Called()
        {
            Mathematics mathematics = m_mathematics.Clone();

            Assert.That(mathematics, Is.Not.Null);
            Assert.That(m_mathematics, Is.Not.EqualTo(mathematics));
        }

        [Test]
        public void When_OnCalculatePlayerProperties_Called()
        {
            Player player = new Player();
            Player opponent = new Player();
            player.Developments.AddRange([new Development(), new Development()]);
            PlayerProperties playerProperties = new PlayerProperties(player, opponent);

            m_mathematics.OnCalculatePlayerProperties(playerProperties);

            Assert.That(playerProperties.VictoryPoints, Is.EqualTo(6));
        }

        private Mathematics m_mathematics;
    }
}
