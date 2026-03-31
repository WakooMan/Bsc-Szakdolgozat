using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.PlayerTurnStates
{
    public class PickCardState : IPlayerTurnState
    {

        public PickCardState(IGameContext gameContext)
        {
            m_gameContext = gameContext;
        }

        public async Task ExecuteTurnState()
        {
            PlayerActionWrapper playerAction;
            do
            {
                playerAction = m_gameContext.PlayerActionReceiver.ReceivePlayerAction(CurrentPlayer, Composition.AvailableCards.Select(card => { 
                    PickCard pickCard = new PickCard(CurrentPlayer, card);
                    return new PlayerActionWrapper(pickCard, pickCard.CanPerform(m_gameContext).GetAwaiter().GetResult());
                }).ToList());
            } while (!playerAction.CanPerform);

            await playerAction.PlayerAction.DoPlayerAction(m_gameContext);
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return new MakeActionDecision(m_gameContext);
        }

        private ICardComposition Composition => m_gameContext.AgeHandler.CurrentAge.Composition;
        private Player CurrentPlayer => m_gameContext.TurnHandler.CurrentPlayer;

        private readonly IGameContext m_gameContext;
    }
}
