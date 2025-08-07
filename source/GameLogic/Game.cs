using GameLogic.Elements;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using GameLogic.GameStates;
using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic
{
    [Export(typeof(IGame))]
    public class Game: IGame
    {
        private List<Player> m_players;
        private readonly IGameContext m_gameContext;
        private bool m_isInitialized = false;
        public IGameState CurrentState { get; private set; }
        public IReadOnlyList<Player> Players => m_players;
        public bool IsInitialized => m_isInitialized;

        [ImportingConstructor]
        public Game(IGameContext gameContext)
        {
            ArgumentChecker.CheckNull(gameContext, nameof(gameContext));

            m_gameContext = gameContext;
            m_players = new List<Player>();
            CurrentState = new EndGameState();
            m_isInitialized = false;
        }

        public void GameLoop()
        {
            ArgumentChecker.CheckPredicateForOperation(() => !m_isInitialized, "Cannot start an uninitialized game!");

            while (CurrentState is not EndGameState)
            {
                CurrentState.DoStateAction();
                CurrentState = CurrentState.GetNextState();
            }

            m_isInitialized = false;
        }

        public void Initialize(string player1, string player2, ICollection<Wonder> wonders, ICollection<Development> developments)
        {
            if (!m_isInitialized)
            {
                m_players = [new Player(player1), new Player(player2)];
                m_gameContext.Initialize(m_players, wonders, developments);
                CurrentState = new ChooseWonderState(m_gameContext);
                m_isInitialized = true;
            }
        }
    }
}
