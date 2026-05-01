using SevenWonders.Common;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.GameStates;
using SevenWonders.Game.Logic.Interfaces;

namespace SevenWonders.Game.Logic
{
    public class Game : IGame
    {
        private List<Player> m_players;
        private readonly IGameContext m_gameContext;
        private bool m_isInitialized = false;

        public IGameState CurrentState { get; private set; }
        public IReadOnlyList<Player> Players => m_players;
        public bool IsInitialized => m_isInitialized;
        public IGameContext Context => m_gameContext;

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

            ICollection<Development> developments = Context.RandomGenerator.ReceiveRandomElements(Context.DevelopmentList.Developments, 5);
            Context.DevelopmentList.Developments.RemoveAll(developments.Contains);
            Context.MilitaryBoard.Initialize(m_players, developments);

            GameLog.Info("GameLoop started.");
            m_gameContext.EventManager.Publish(new OnGameInitialized(m_gameContext));
            m_gameContext.EventManager.Publish(new OnGameStarted(m_players));

            while (CurrentState is not EndGameState)
            {
                GameLog.Info($"Executing state: {CurrentState.GetType().Name}");
                CurrentState.DoStateAction();
                CurrentState = CurrentState.GetNextState();
            }

            GameLog.Info("GameLoop ended.");
            m_isInitialized = false;
        }

        public void Initialize(IRandomGenerator randomGenerator, (string name, IPlayerActionReceiver actionReceiver) player1, (string name, IPlayerActionReceiver actionReceiver) player2, int startingPlayerId = 1)
        {
            if (!m_isInitialized)
            {
                GameLog.Info($"Initializing game: Player1={player1.name}, Player2={player2.name}, StartingPlayerId={startingPlayerId}");
                ArgumentChecker.CheckPredicateForOperation(() => startingPlayerId != 1 && startingPlayerId != 2, "startingPlayerId must be 1 or 2.");
                var p1 = new Player(player1.actionReceiver, player1.name, 1, 7);
                var p2 = new Player(player2.actionReceiver, player2.name, 2, 7);
                m_players = startingPlayerId == 1 ? [p1, p2] : [p2, p1];
                m_gameContext.Initialize(m_players, randomGenerator);
                CurrentState = new ChooseWonderState(m_gameContext, randomGenerator, m_players);
                m_isInitialized = true;
                GameLog.Info("Game initialized successfully.");
            }
        }
    }
}
