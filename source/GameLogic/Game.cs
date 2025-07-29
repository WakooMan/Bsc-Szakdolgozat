using GameLogic.Elements;
using GameLogic.GameStates;

namespace GameLogic
{
    public class Game
    {
        private readonly List<Player> m_players;
        public IGameState CurrentState { get; private set; }
        public IReadOnlyList<Player> Players => m_players;


        public Game(string player1, string player2, IGameContext gameContext)
        {
            m_players = [new Player(player1), new Player(player2)];
            gameContext.ChooseWonderHandler.SetPlayers(m_players);
            gameContext.TurnHandler.SetPlayers(m_players);
            CurrentState = new ChooseWonderState(gameContext);
        }

        public void GameLoop()
        {
            while (CurrentState != null)
            {
                CurrentState.DoStateAction();
                CurrentState = CurrentState.GetNextState();
            }
        }
    }
}
