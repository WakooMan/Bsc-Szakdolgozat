using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class BuildFreeFromDroppedCards : Effect
    {
        public BuildFreeFromDroppedCards() { }
        public override void Apply(IGameContext gameContext)
        {
            IPlayerAction playerAction = gameContext.PlayerActionReceiver.ReceivePlayerAction(gameContext.TurnHandler.CurrentPlayer, gameContext.DroppedCardList.Cards.Select(card => (IPlayerAction)new ChooseCardAction(gameContext, card)).ToArray());
            playerAction.DoPlayerAction();
        }

        public override Effect Clone()
        {
            return new BuildFreeFromDroppedCards();
        }
    }
}
