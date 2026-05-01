using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class CheaperBuildingTests
    {
        [SetUp]
        public void Setup()
        {
            m_cheaperBuilding = new CheaperBuilding();
        }

        [Test]
        public void When_Clone_Called()
        {
            m_cheaperBuilding.AmountOfResources = 5;
            m_cheaperBuilding.BuildingType = "test";
            CheaperBuilding cheaperBuilding = m_cheaperBuilding.Clone();

            Assert.That(cheaperBuilding, Is.Not.Null);
            Assert.That(m_cheaperBuilding, Is.Not.EqualTo(cheaperBuilding));
            Assert.That(cheaperBuilding.AmountOfResources, Is.EqualTo(m_cheaperBuilding.AmountOfResources));
            Assert.That(cheaperBuilding.BuildingType, Is.EqualTo(m_cheaperBuilding.BuildingType));
        }

        private CheaperBuilding m_cheaperBuilding;
    }
}
