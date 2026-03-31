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
            PlayerActionWrapper playerActionWrapper = gameContext.PlayerActionReceiver.ReceivePlayerAction(
                gameContext.TurnHandler.CurrentPlayer,
                droppedCardList.Cards.Select(card => new PlayerActionWrapper(new ChooseCardAction(card), true)).ToList());
            if (playerActionWrapper.CanPerform)
            {
                await playerActionWrapper.PlayerAction.DoPlayerAction(gameContext);
            }
        }

        public override BuildFreeFromDroppedCards Clone()
        {
            return new BuildFreeFromDroppedCards();
        }
    }
}
