using GameLogic.Ages;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Guilds;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.GameCards
{
    public class PurpleCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_good = Substitute.For<Good>();
            m_guild = Substitute.For<Guild>();
            m_purpleCard = new PurpleCard()
            {
                Name = "testName",
                PreviousBuilding = "testPrevious",
                GoodCost = new List<Good> { m_good },
                Age = AgesEnum.I,
                GuildObj = m_guild
            };
        }

        [Test]
        public void When_Initialized_With_Default_Constructor()
        {
            m_purpleCard = new PurpleCard();

            Assert.That(m_purpleCard, Is.Not.Null);
            Assert.That(m_purpleCard.BuildingType, Is.EqualTo(typeof(PurpleCard).Name));
            Assert.That(m_purpleCard.Name, Is.EqualTo(string.Empty));
            Assert.That(m_purpleCard.PreviousBuilding, Is.EqualTo(string.Empty));
            Assert.That(m_purpleCard.GoodCost, Is.Not.Null);
            Assert.That(m_purpleCard.GoodCost.Count, Is.EqualTo(0));
            Assert.That(m_purpleCard.Age, Is.EqualTo(AgesEnum.None));

            Assert.That(m_purpleCard.GuildObj, Is.Not.Null);
            Assert.That(m_purpleCard.GuildObj.GetType(), Is.EqualTo(typeof(DefaultGuild)));
        }

        [Test]
        public void When_Clone_Called()
        {
            PurpleCard yellowCard = m_purpleCard.Clone();

            Assert.That(yellowCard, Is.Not.Null);
            Assert.That(yellowCard.Equals(m_purpleCard), Is.False);
            Assert.That(yellowCard.Name, Is.EqualTo(m_purpleCard.Name));
            Assert.That(yellowCard.PreviousBuilding, Is.EqualTo(m_purpleCard.PreviousBuilding));
            Assert.That(yellowCard.GoodCost, Is.Not.Null);
            Assert.That(yellowCard.GoodCost.Count, Is.EqualTo(m_purpleCard.GoodCost.Count));
            m_good.Received(1).Clone();
            Assert.That(yellowCard.Age, Is.EqualTo(m_purpleCard.Age));

            m_guild.Received(1).Clone();
        }

        private Guild m_guild;
        private Good m_good;
        private PurpleCard m_purpleCard;
    }
}
