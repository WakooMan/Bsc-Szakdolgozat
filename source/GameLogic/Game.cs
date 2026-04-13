using GameLogic.Elements;
using GameLogic.Events.GameEvents;
using GameLogic.GameStates;
using GameLogic.Interfaces;
using SevenWonders.Common;

namespace GameLogic
{
    public class Game: IGame
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

        public async void GameLoop()
        {
            ArgumentChecker.CheckPredicateForOperation(() => !m_isInitialized, "Cannot start an uninitialized game!");

            await m_gameContext.EventManager.PublishAsync(new OnGameInitialized(m_gameContext));
            await m_gameContext.EventManager.PublishAsync(new OnGameStarted(m_players));

            while (CurrentState is not EndGameState)
            {
                await CurrentState.DoStateAction();
                CurrentState = CurrentState.GetNextState();
            }

            m_isInitialized = false;
        }

        public void Initialize((string name, IPlayerActionReceiver actionReceiver) player1, (string name, IPlayerActionReceiver actionReceiver) player2)
        {
            if (!m_isInitialized)
            {
                m_players = [new Player(player1.actionReceiver, player1.name, 1, 7), new Player(player2.actionReceiver, player2.name, 2, 7)];
                m_gameContext.Initialize(m_players);
                CurrentState = new ChooseWonderState(m_gameContext);
                m_isInitialized = true;
            }
        }
    }
}
