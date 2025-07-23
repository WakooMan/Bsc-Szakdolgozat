using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.GameStates;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using SevenWonders.Common;

namespace GameLogic
{
    public class Game
    {
        private readonly List<Player> m_players;
        public IGameState CurrentState { get; private set; }
        public IReadOnlyList<Player> Players => m_players;


        public Game(string player1, string player2, IPlayerActionReceiver playerActionReceiver, IWonderList wonderList)
        {
            m_players = new List<Player>();
            m_players.Add(new Player(player1));
            m_players.Add(new Player(player2));
            CurrentState = new ChooseWonderState(new ChooseWonderHandler(new RandomGenerator() ,playerActionReceiver, wonderList, m_players), playerActionReceiver);
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
