using GameLogic.Elements.GameCards;
using GameLogic.Events.GameEvents;
using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class BuildFreeFromDroppedCards : Effect
    {
        public BuildFreeFromDroppedCards() { }
        public override async Task Apply(IGameContext gameContext)
        {
            ICardList droppedCardList = gameContext.DroppedCardList ?? throw new InvalidOperationException($"{nameof(gameContext.DroppedCardList)} cannot be null in IGameContext object with parameter name: {nameof(gameContext)}!");
            await gameContext.EventManager.PublishAsync(new OnChooseObjects("Choose Dropped Card", droppedCardList.Cards.Select(card => card.Name).ToArray(), true));
            await gameContext.PlayerActionHandler.HandlePlayerActions(gameContext,
                  gameContext.TurnHandler.CurrentPlayer, 
                  droppedCardList.Cards.Select(card => (IPlayerAction)new ChooseDroppedCardAction(card)).ToList());
        }

        public override BuildFreeFromDroppedCards Clone()
        {
            return new BuildFreeFromDroppedCards();
        }
    }
}
