using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Logic.PlayerActions;

namespace SevenWonders.Game.Logic.Elements.Effects
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
            var enemyCards = opponent.Cards.Where(card => card.BuildingType == CardType).ToList();
            if (enemyCards.Count > 0)
            {
                gameContext.EventManager.Publish(new OnChooseObjects("Ellenfél kártyájának kidobása", enemyCards.Select(card => card.Name).ToArray()));
                var result = gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, owner, enemyCards.Select(card =>
                {
                    IPlayerAction dropCard = new DropCard(opponent, owner, card);
                    return dropCard;
                }).ToArray());

                gameContext.EventManager.Publish(new OnObjectChosen((result.completed && result.playerAction is DropCard dropCard) ? dropCard.Name : string.Empty));
            }
        }
    }
}
