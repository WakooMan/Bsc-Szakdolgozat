using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class BuildFreeFromDroppedCards : Effect
    {
        public BuildFreeFromDroppedCards() { }
        public override void Apply(IGameContext gameContext)
        {
            IPlayerAction playerAction = gameContext.PlayerActionReceiver.ReceivePlayerAction(gameContext.TurnHandler.CurrentPlayer, gameContext.DroppedCardList?.Cards.Select(card => (IPlayerAction)new ChooseCardAction(card)).ToArray() ?? throw new InvalidOperationException($"{nameof(gameContext.DroppedCardList)} cannot be null in IGameContext object with parameter name: {nameof(gameContext)}!"));
            if (playerAction.CanPerform(gameContext))
            {
                playerAction.DoPlayerAction(gameContext);
            }
        }

        public override Effect Clone()
        {
            return new BuildFreeFromDroppedCards();
        }
    }
}
