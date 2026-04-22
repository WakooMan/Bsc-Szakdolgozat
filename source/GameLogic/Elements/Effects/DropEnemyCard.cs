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

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            var enemyCards = opponent.Cards.Where(card => card.BuildingType == CardType);
            if (enemyCards.Count() > 0)
            {
                gameContext.EventManager.Publish(new OnChooseObjects("Ellenfél kártyájának kidobása", opponent.Cards.Select(card => card.Name).ToArray(), true));
                gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, owner, enemyCards.Select(card =>
                {
                    IPlayerAction dropCard = new DropCard(opponent, owner, card);
                    return dropCard;
                }).ToArray());
                gameContext.EventManager.Publish(new OnObjectChosen(opponent.Cards.Select(card => card.Name).ToArray(), true));
            }
        }
    }
}
