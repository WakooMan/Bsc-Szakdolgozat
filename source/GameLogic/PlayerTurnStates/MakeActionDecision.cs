using GameLogic.Elements;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using System.Runtime.CompilerServices;

namespace GameLogic.PlayerTurnStates
{
    public class MakeActionDecision : IPlayerTurnState
    {
        public bool GoToPrevState { get; set; }

        public MakeActionDecision(IGameContext gameContext)
        {
            m_gameContext = gameContext;
            GoToPrevState = false;
        }

        public async Task ExecuteTurnState()
        {
            Action<OnCardUnpicked> action = (args) => GoToPrevState = true;
            m_gameContext.EventManager.Subscribe(action);
            List<IPlayerAction> playerActions =
            [
                new TurnDecision(new UnpickCard(CurrentPlayer)), new TurnDecision(new BuildCard()), new TurnDecision(new SellCard(CurrentPlayer)),
                new TurnDecision(new BuildWonderProcess(CurrentPlayer, CurrentPlayer.Wonders.Select(wonder => new BuildWonder(wonder)).ToList())),
            ];
            await m_gameContext.PlayerActionHandler.HandlePlayerActionsCompleted(m_gameContext, CurrentPlayer, playerActions);
            m_gameContext.EventManager.Unsubscribe(action);
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return GoToPrevState ? new PickCardState(m_gameContext) : new EndTurn();
        }

        private Player CurrentPlayer => m_gameContext.TurnHandler.CurrentPlayer;

        private readonly IGameContext m_gameContext;
    }
}
