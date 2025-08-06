using GameLogic;
using GameLogic.Elements.Effects;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Wonders;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Wonders
{
    public class WonderTests
    {
        [SetUp]
        public void Setup()
        {
            m_effect = Substitute.For<Effect>();
            m_good = Substitute.For<Good>();
            m_wonder = new Wonder()
            {
                Name = "test",
                Effects = new List<Effect>() { m_effect },
                HasBeenBuilt = true,
                GoodCost = new List<Good>() { m_good }
            };
        }

        [Test]
        public void When_Initialized_With_Default_Constructor()
        {
            m_wonder = new Wonder();
            Assert.That(m_wonder.Name, Is.EqualTo(string.Empty));
            Assert.That(m_wonder.Effects, Is.Not.Null);
            Assert.That(m_wonder.Effects.Count, Is.EqualTo(0));
            Assert.That(m_wonder.GoodCost, Is.Not.Null);
            Assert.That(m_wonder.GoodCost.Count, Is.EqualTo(0));
            Assert.That(m_wonder.BuildingType, Is.EqualTo(typeof(Wonder).Name));
            Assert.That(m_wonder.HasBeenBuilt, Is.False);
        }

        [Test]
        public void When_Clone_Called()
        {
            Wonder clonedWonder = m_wonder.Clone();

            Assert.That(clonedWonder.Equals(m_wonder), Is.False);
            Assert.That(clonedWonder.Name, Is.EqualTo(m_wonder.Name));

            Assert.That(clonedWonder.Effects, Is.Not.Null);
            Assert.That(clonedWonder.Effects.Equals(m_wonder.Effects), Is.False);
            Assert.That(clonedWonder.Effects.Count, Is.EqualTo(m_wonder.Effects.Count));
            m_effect.Received(1).Clone();

            Assert.That(clonedWonder.GoodCost, Is.Not.Null);
            Assert.That(clonedWonder.GoodCost.Equals(m_wonder.GoodCost), Is.False);
            Assert.That(clonedWonder.GoodCost.Count, Is.EqualTo(m_wonder.GoodCost.Count));
            m_good.Received(1).Clone();

            Assert.That(clonedWonder.BuildingType, Is.EqualTo(m_wonder.BuildingType));
            Assert.That(clonedWonder.HasBeenBuilt, Is.EqualTo(m_wonder.HasBeenBuilt));
        }

        [Test]
        public void When_OnBuilt_Called()
        {
            IGameContext gameContext = Substitute.For<IGameContext>();
            m_wonder.OnBuilt(gameContext);

            m_effect.Received(1).Apply(gameContext);
        }



        private Wonder m_wonder;
        private Effect m_effect;
        private Good m_good;
    }
}
