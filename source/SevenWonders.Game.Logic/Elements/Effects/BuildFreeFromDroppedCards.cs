using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.PlayerActions;

namespace SevenWonders.Game.Logic.Elements.Effects
{
    public class BuildFreeFromDroppedCards : Effect
    {
        public BuildFreeFromDroppedCards() { }
        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            ICardList droppedCardList = gameContext.DroppedCardList ?? throw new InvalidOperationException($"{nameof(gameContext.DroppedCardList)} cannot be null in IGameContext object with parameter name: {nameof(gameContext)}!");
            if (droppedCardList.Cards.Count > 0)
            {
                gameContext.EventManager.Publish(new OnChooseObjects("Válassz az eldobott kártyákból", droppedCardList.Cards.Select(card => card.Name).ToArray(), true));
                gameContext.PlayerActionHandler.HandlePlayerActions(gameContext,
                      owner,
                      droppedCardList.Cards.Select(card => (IPlayerAction)new ChooseDroppedCardAction(card)).ToList());
            }
        }

        public override BuildFreeFromDroppedCards Clone()
        {
            return new BuildFreeFromDroppedCards();
        }
    }
}
