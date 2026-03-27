using GameLogic.Elements;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
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

            IPlayerAction playerAction;
            do
            {
                playerAction = m_gameContext.PlayerActionReceiver.ReceivePlayerAction(CurrentPlayer, playerActions.Select(decision => (IPlayerAction)decision).ToList());
            } while (!GoToPrevState && !await playerAction.CanPerform(m_gameContext));

            if (!GoToPrevState)
            {
                await playerAction.DoPlayerAction(m_gameContext);
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
