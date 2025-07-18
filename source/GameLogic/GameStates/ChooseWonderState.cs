using GameLogic.Handlers;
using GameLogic.Interfaces;

namespace GameLogic.GameStates
{
    public class ChooseWonderState : IGameState
    {
        private readonly IChooseWonderHandler m_chooseWonderHandler;
        private readonly IPlayerActionReceiver m_playerActionReceiver;

        public ChooseWonderState(IChooseWonderHandler chooseWonderHandler, IPlayerActionReceiver playerActionReceiver)
        {
            m_chooseWonderHandler = chooseWonderHandler;
            m_playerActionReceiver = playerActionReceiver;
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
            return new PlayingState(m_playerActionReceiver, m_chooseWonderHandler.Players);
        }
    }
}
