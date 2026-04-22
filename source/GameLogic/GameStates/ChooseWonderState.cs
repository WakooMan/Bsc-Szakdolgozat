using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using SevenWonders.Common;

namespace GameLogic.GameStates
{
    public class ChooseWonderState : IGameState
    {

        public ChooseWonderState(IGameContext gameContext, IRandomGenerator randomGenerator, ICollection<Player> players)
        {
            m_gameContext = gameContext;
            m_randomGenerator = randomGenerator;
            m_players = players;
        }

        public void DoStateAction()
        {
            ICollection<Wonder> wonders = m_randomGenerator.ReceiveRandomElements(m_gameContext.WonderList.Wonders, 8);
            m_gameContext.WonderList.Wonders.RemoveAll(wonders.Contains);
            m_gameContext.ChooseWonderHandler.Initialize(m_players, wonders, m_gameContext);
            while (!m_gameContext.ChooseWonderHandler.WondersChosen)
            {
                m_gameContext.ChooseWonderHandler.ChooseWonder();
            }
        }

        public IGameState GetNextState()
        {
            return new PlayingState(m_gameContext);
        }

        private readonly IRandomGenerator m_randomGenerator;
        private readonly IGameContext m_gameContext;
        private readonly ICollection<Player> m_players;
    }
}
