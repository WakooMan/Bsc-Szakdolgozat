using GameLogic.Elements;
using GameLogic.Handlers;
using GameLogic.Handlers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Game
    {
        public bool IsGameOver { get; private set; }
        public AgeHandler AgeHandler { get; }
        public TurnHandler TurnHandler { get; }

        public Game(string player1, string player2)
        {
            AgeHandler = new AgeHandler(new CardCompositionFileHandlerFactory());
            TurnHandler = new TurnHandler(new Player[] { new Player(player1), new Player(player2) });
        }

        public void GameLoop()
        {
            while (!IsGameOver)
            {
                // DO player turn

                // Check If player won in the turn.

                if (AgeHandler.CurrentAge.IsAgeOver)
                {
                    IsGameOver = !AgeHandler.NextAge();
                }

                TurnHandler.NextPlayer();
            }
        }
    }
}
