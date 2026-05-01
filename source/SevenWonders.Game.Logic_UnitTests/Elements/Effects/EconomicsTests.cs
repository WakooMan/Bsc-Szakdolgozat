using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events.GameEvents;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class EconomicsTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_player = new Player();
            m_opponent = new Player();
            m_economics = new Economics();
        }

        [Test]
        public void When_Clone_Called()
        {
            Economics economics = m_economics.Clone();

            Assert.That(economics, Is.Not.Null);
            Assert.That(m_economics, Is.Not.EqualTo(economics));
        }

        [TestCase(10, 4)]
        [TestCase(4, 4)]
        [TestCase(4, 5)]
        public void When_Apply_Called_Opponent_Builds_Card(int buildCost, int moneyCost)
        {
            Card card = new RedCard() { MoneyCost = moneyCost };
            OnCardBuilt onCardBuilt = new OnCardBuilt(card, m_opponent, buildCost, true);

            m_economics.Apply(m_gameContext, m_player, m_opponent);

            m_opponent.OnBuildCard(onCardBuilt);
            Assert.That(m_opponent.Money, Is.EqualTo(Math.Max(0, buildCost - card.MoneyCost)));
        }

        private IGameContext m_gameContext;
        private Player m_player;
        private Player m_opponent;
        private Economics m_economics;
    }
}
