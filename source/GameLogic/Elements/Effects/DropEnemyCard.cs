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

        public override async Task Apply(IGameContext gameContext)
        {
            Player currentPlayer = gameContext.TurnHandler.CurrentPlayer;
            Player opponentPlayer = gameContext.TurnHandler.OpponentPlayer;

            await gameContext.PlayerActionHandler.HandlePlayerActionsCompleted(gameContext, currentPlayer, opponentPlayer.Cards.Where(card => card.BuildingType == CardType).Select(card => {
                IPlayerAction dropCard = new DropCard(opponentPlayer, card);
                return dropCard;
            }).ToArray());
        }
    }
}
