using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.GameStates;
using SevenWonders.Game.Logic.Handlers;
using NSubstitute;
using SevenWonders.Common;
using SevenWonders.Game.Logic.Elements.Wonders;

namespace GameLogic_UnitTests.GameStates
{
    public class ChooseWonderStateTests
    {
        [SetUp]
        public void Setup()
        {
            m_chooseWonderHandler = Substitute.For<IChooseWonderHandler>();
            m_gameContext = Substitute.For<IGameContext>();
            m_randomGenerator = Substitute.For<IRandomGenerator>();
            m_wonderList = Substitute.For<IWonderList>();
            m_wonderList.Wonders.Returns([new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder()
                                         ]);
            m_gameContext.WonderList.Returns(m_wonderList);
            m_players = new List<Player> { new Player(), new Player() };
            m_gameContext.ChooseWonderHandler.Returns(m_chooseWonderHandler);
            m_chooseWonderState = new ChooseWonderState(m_gameContext, m_randomGenerator, m_players);
        }

        [Test]
        public void When_DoStateAction_Called_And_WonderChosen_Returns_True()
        {
            m_chooseWonderHandler.WondersChosen.Returns(true);

            m_chooseWonderState.DoStateAction();

            m_chooseWonderHandler.DidNotReceive().ChooseWonder();
        }

        [Test]
        public void When_DoStateAction_Called_And_WonderChosen_Returns_False_Once()
        {
            m_chooseWonderHandler.WondersChosen.Returns(false, true);

            m_chooseWonderState.DoStateAction();

            m_chooseWonderHandler.Received(1).ChooseWonder();
        }

        [Test]
        public void When_GetNextState_Called()
        {
            var turnState = m_chooseWonderState.GetNextState();
            Assert.That(turnState is PlayingState, Is.True);
        }

        private IChooseWonderHandler m_chooseWonderHandler;
        private IGameContext m_gameContext;
        private IRandomGenerator m_randomGenerator;
        private ICollection<Player> m_players;
        private ChooseWonderState m_chooseWonderState;
        private IWonderList m_wonderList;
    }
}
