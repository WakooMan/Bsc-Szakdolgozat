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
            IPlayerAction action = gameContext.PlayerActionReceiver.ReceivePlayerAction(currentPlayer, opponentPlayer.Cards.Where(card => card.BuildingType == CardType).Select(card => new DropCard(opponentPlayer, card)).ToArray());
            if (await action.CanPerform(gameContext))
            {
                await action.DoPlayerAction(gameContext);
            }
        }
    }
}
