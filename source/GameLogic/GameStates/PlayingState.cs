using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;
using GameLogic.Handlers.Factories;

namespace GameLogic.GameStates
{
    public class PlayingState : IGameState
    {
        public bool IsGameOver { get; private set; }
        public IAgeHandler AgeHandler { get; }
        public ITurnHandler TurnHandler { get; }

        public PlayingState(ICollection<Player> players)
        {
            AgeHandler = new AgeHandler(new CardCompositionFactory(new CardCompositionFileHandlerFactory(), new CardNodeFactory()), new CardList());
            TurnHandler = new TurnHandler(players);
        }
        
        public void DoStateAction()
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

        public IGameState GetNextState()
        {
            return null;
        }
    }
}
