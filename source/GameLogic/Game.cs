using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.GameStates;
using System.ComponentModel.Composition;

namespace GameLogic
{
    [Export(typeof(IGame))]
    public class Game: IGame
    {
        private List<Player> m_players;
        private readonly IGameContext m_gameContext;
        private bool m_isInitialized = false;
        public IGameState? CurrentState { get; private set; }
        public IReadOnlyList<Player> Players => m_players;

        [ImportingConstructor]
        public Game(IGameContext gameContext)
        {
            m_gameContext = gameContext;
            m_players = new List<Player>();
            CurrentState = null;
            m_isInitialized = false;
        }

        public void GameLoop()
        {
            if (!m_isInitialized)
            {
                throw new InvalidOperationException("Cannot start an uninitialized game!");
            }

            while (CurrentState != null)
            {
                CurrentState.DoStateAction();
                CurrentState = CurrentState.GetNextState();
            }

            m_isInitialized = false;
        }

        public void Initialize(string player1, string player2, ICollection<Wonder> wonders)
        {
            if (!m_isInitialized)
            {
                m_players = [new Player(player1), new Player(player2)];
                m_gameContext.Initialize(m_players, wonders);
                CurrentState = new ChooseWonderState(m_gameContext);
                m_isInitialized = true;
            }
        }
    }
}
