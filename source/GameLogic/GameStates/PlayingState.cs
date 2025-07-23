using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;
using GameLogic.Handlers.Factories;
using GameLogic.Interfaces;
using GameLogic.PlayerTurnStates;

namespace GameLogic.GameStates
{
    public class PlayingState : IGameState
    {
        public bool IsGameOver { get; private set; }
        public IAgeHandler AgeHandler { get; }
        public ITurnHandler TurnHandler { get; }
        public IPlayerActionReceiver PlayerActionReceiver { get; }
        public IEventManager EventManager { get; }

        public PlayingState(IPlayerActionReceiver playerActionReceiver, ICollection<Player> players)
        {
            PlayerActionReceiver = playerActionReceiver;
            EventManager = new EventManager();
            AgeHandler = new AgeHandler(new CardCompositionFactory(new CardCompositionFileHandlerFactory(), new CardNodeFactory()), new CardList());
            TurnHandler = new TurnHandler(players);
        }
        
        public void DoStateAction()
        {
            while (!IsGameOver)
            {
                IPlayerTurnState playerTurnState = new PickCardState(PlayerActionReceiver, TurnHandler.CurrentPlayer, AgeHandler.CurrentAge.Composition);

                while (playerTurnState is not EndTurn)
                {
                    playerTurnState.ExecuteTurnState(EventManager);
                    playerTurnState = playerTurnState.GetNextTurnState();
                }

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
