using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Handlers;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class GetMoneyForCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_player = new Player();
            m_player.Cards.AddRange(new RedCard(), new RedCard(), new GreenCard(), new YellowCard());
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_getMoneyForCard = new GetMoneyForCard();
        }

        [Test]
        public void When_Clone_Called()
        {
            m_getMoneyForCard.MoneyPerCard = 5;
            m_getMoneyForCard.CardType = "Test";
            GetMoneyForCard getMoneyForCard = m_getMoneyForCard.Clone();

            Assert.That(getMoneyForCard, Is.Not.Null);
            Assert.That(m_getMoneyForCard, Is.Not.EqualTo(getMoneyForCard));
            Assert.That(getMoneyForCard.MoneyPerCard, Is.EqualTo(m_getMoneyForCard.MoneyPerCard));
            Assert.That(getMoneyForCard.CardType, Is.EqualTo(m_getMoneyForCard.CardType));
        }



        [TestCase(5, nameof(RedCard), 10)]
        [TestCase(4, nameof(RedCard), 8)]
        [TestCase(5, nameof(GreenCard), 5)]
        [TestCase(3, nameof(YellowCard), 3)]
        public void When_Apply_Called(int moneyPerCard, string cardType, int expectedMoney)
        {
            m_getMoneyForCard.MoneyPerCard = moneyPerCard;
            m_getMoneyForCard.CardType = cardType;

            m_getMoneyForCard.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            Assert.That(m_player.Money, Is.EqualTo(Math.Max(0, expectedMoney)));
        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private Player m_player;
        private GetMoneyForCard m_getMoneyForCard;
    }
}
