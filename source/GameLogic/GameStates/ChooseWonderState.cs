using GameLogic.Events.GameEvents;

namespace GameLogic.GameStates
{
    public class ChooseWonderState : IGameState
    {
        private readonly IGameContext m_gameContext;

        public ChooseWonderState(IGameContext gameContext)
        {
            m_gameContext = gameContext;
        }

        public async Task DoStateAction()
        {
            while (!m_gameContext.ChooseWonderHandler.WondersChosen)
            {
                await m_gameContext.ChooseWonderHandler.ChooseWonder();
            }
        }

        public IGameState GetNextState()
        {
            return new PlayingState(m_gameContext);
        }
    }
}
