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
            List<TurnDecision> playerActions =
            [
                new TurnDecision(new UnpickCard(CurrentPlayer)), new TurnDecision(new BuildCard()), new TurnDecision(new SellCard(CurrentPlayer)),
                .. CurrentPlayer.Wonders.Select(wonder => new TurnDecision(new BuildWonder(wonder))),
            ];

            PlayerActionWrapper playerAction;
            do
            {
                playerAction = m_gameContext.PlayerActionReceiver.ReceivePlayerAction(CurrentPlayer, playerActions.Select(decision => new PlayerActionWrapper(decision, decision.CanPerform(m_gameContext).GetAwaiter().GetResult())).ToList());
            } while (!GoToPrevState && !playerAction.CanPerform);

            if (!GoToPrevState)
            {
                await playerAction.PlayerAction.DoPlayerAction(m_gameContext);
            }

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
