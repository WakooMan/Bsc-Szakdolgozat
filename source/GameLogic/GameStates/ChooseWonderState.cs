using GameLogic.Handlers;

namespace GameLogic.GameStates
{
    public class ChooseWonderState : IGameState
    {
        private readonly IChooseWonderHandler m_chooseWonderHandler;

        public ChooseWonderState(IChooseWonderHandler chooseWonderHandler)
        {
            m_chooseWonderHandler = chooseWonderHandler;
        }

        public void DoStateAction()
        {
            while (!m_chooseWonderHandler.WondersChosen)
            {
                m_chooseWonderHandler.ChooseWonder();
            }
        }

        public IGameState GetNextState()
        {
            return new PlayingState(m_chooseWonderHandler.Players);
        }
    }
}
