using GameLogic.Elements.GameCards;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class BuildFreeFromDroppedCards : Effect
    {
        public BuildFreeFromDroppedCards() { }
        public override async Task Apply(IGameContext gameContext)
        {
            ICardList droppedCardList = gameContext.DroppedCardList ?? throw new InvalidOperationException($"{nameof(gameContext.DroppedCardList)} cannot be null in IGameContext object with parameter name: {nameof(gameContext)}!");

            await gameContext.PlayerActionHandler.HandlePlayerActionsCompleted(gameContext, gameContext.TurnHandler.CurrentPlayer, droppedCardList.Cards.Select(card => (IPlayerAction)new ChooseCardAction(card)).ToList());
        }

        public override BuildFreeFromDroppedCards Clone()
        {
            return new BuildFreeFromDroppedCards();
        }
    }
}
