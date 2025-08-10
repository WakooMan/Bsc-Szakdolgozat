using GameLogic.Handlers;
using GameLogic.Interfaces;

namespace GameLogic.GameStates
{
    public class ChooseWonderState : IGameState
    {
        private readonly IGameContext m_gameContext;

        public ChooseWonderState(IGameContext gameContext)
        {
            m_gameContext = gameContext;
        }

        public void DoStateAction()
        {
            while (!m_gameContext.ChooseWonderHandler.WondersChosen)
            {
                m_gameContext.ChooseWonderHandler.ChooseWonder();
            }
        }

        public IGameState GetNextState()
        {
            return new PlayingState(m_gameContext);
        }
    }
}
