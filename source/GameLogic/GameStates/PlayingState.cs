using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using GameLogic.GameStructures;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;
using GameLogic.Handlers.Factories;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.GameStates
{
    public class PlayingState : IGameState
    {
        public bool IsGameOver { get; private set; }
        public IAgeHandler AgeHandler { get; }
        public ITurnHandler TurnHandler { get; }
        public IPlayerActionReceiver PlayerActionReceiver { get; }

        public PlayingState(IPlayerActionReceiver playerActionReceiver, ICollection<Player> players)
        {
            PlayerActionReceiver = playerActionReceiver;
            AgeHandler = new AgeHandler(new CardCompositionFactory(new CardCompositionFileHandlerFactory(), new CardNodeFactory()), new CardList());
            TurnHandler = new TurnHandler(players);
            m_playerActions = new Dictionary<Player, Dictionary<ICardNode, List<IPlayerAction>>>();
            FillPlayerActions(players);
            AgeHandler.HandleAgeChanged += (age) => FillPlayerActions(players);
        }
        
        public void DoStateAction()
        {
            while (!IsGameOver)
            {
                PlayerActionReceiver.ReceivePlayerAction(TurnHandler.CurrentPlayer, GetAvailablePlayerActions(TurnHandler.CurrentPlayer)).DoPlayerAction();

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

        private void FillPlayerActions(ICollection<Player> players)
        {
            m_playerActions.Clear();
            foreach (Player player in players)
            {
                m_playerActions.Add(player, new Dictionary<ICardNode, List<IPlayerAction>>());
                List<IPlayerAction> playerActions = new List<IPlayerAction>();
                foreach (ICardNode cardNode in AgeHandler.CurrentAge.Composition.AllCards)
                {
                    playerActions.Add(new BuildCard(cardNode, AgeHandler.CurrentAge, player));
                    playerActions.Add(new SellCard(cardNode, AgeHandler.CurrentAge, player));
                    foreach (Wonder wonder in player.Wonders)
                    {
                        playerActions.Add(new BuildWonder(cardNode, AgeHandler.CurrentAge, wonder, player));
                    }
                    m_playerActions[player].Add(cardNode, playerActions);
                }
            }
        }

        private List<IPlayerAction> GetAvailablePlayerActions(Player player)
        {
            return m_playerActions[TurnHandler.CurrentPlayer].Where((kvp) => AgeHandler.CurrentAge.Composition.AvailableCards.Contains(kvp.Key)).Select(kvp => kvp.Value).Aggregate((a, b) => a.Union(b).ToList());
        }

        private readonly Dictionary<Player, Dictionary<ICardNode, List<IPlayerAction>>> m_playerActions;
    }
}
