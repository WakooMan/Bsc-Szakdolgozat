using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.Military;
using SevenWonders.Game.Logic.Handlers;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Military
{
    public class MilitaryCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_card = new MilitaryCard();
        }

        [Test]
        public void When_Default_Constructor_Called()
        {
            Assert.That(m_card.EnemyLoseMoney, Is.Not.Null);
            Assert.That(m_card.EnemyLoseMoney.Money, Is.EqualTo(0));
            Assert.That(m_card.VictoryPoints, Is.Not.Null);
            Assert.That(m_card.VictoryPoints.Points, Is.EqualTo(0));
            Assert.That(m_card.IndexEnd, Is.EqualTo(0));
            Assert.That(m_card.IndexStart, Is.EqualTo(0));
        }

        [Test]
        public void When_Apply_Called()
        {
            Player owner = new Player() { Name = "Owner", Id = 1 };
            Player opponent = new Player() { Name = "Opponent", Id = 2 };
            m_card.OwnerId = 1;
            m_card.OpponentId = 2;
            m_card.EnemyLoseMoney.Money = 5;
            IGameContext gameContext = Substitute.For<IGameContext>();
            ITurnHandler turnHandler = Substitute.For<ITurnHandler>();
            turnHandler.GetPlayer(1).Returns(owner);
            turnHandler.GetPlayer(2).Returns(opponent);
            gameContext.TurnHandler.Returns(turnHandler);

            m_card.Apply(gameContext);

            Assert.That(opponent.MilitaryCards.Contains(m_card), Is.True);
        }

        private MilitaryCard m_card;
    }
}
