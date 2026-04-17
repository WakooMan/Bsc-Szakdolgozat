using GameLogic.Events.GameEvents;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class DropEnemyCard : Effect
    {
        public string CardType { get; set; }

        public DropEnemyCard()
        {
            CardType = string.Empty;
        }

        private DropEnemyCard(DropEnemyCard dropEnemyCard)
        {
            CardType = dropEnemyCard.CardType;
        }

       

        public override DropEnemyCard Clone()
        {
            return new DropEnemyCard(this);
        }

        public override async Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            await gameContext.EventManager.PublishAsync(new OnChooseObjects("Ellenfél kártyájának kidobása", opponent.Cards.Select(card => card.Name).ToArray(), true));
            await gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, owner, opponent.Cards.Where(card => card.BuildingType == CardType).Select(card => {
                IPlayerAction dropCard = new DropCard(opponent, owner, card);
                return dropCard;
            }).ToArray());
            await gameContext.EventManager.PublishAsync(new OnObjectChosen(opponent.Cards.Select(card => card.Name).ToArray(), true));
        }
    }
}
