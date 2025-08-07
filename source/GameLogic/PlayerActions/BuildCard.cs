using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class BuildCard : IPlayerAction
    {
        public BuildCard() { }

        public void DoPlayerAction(IGameContext gameContext)
        {
            Player player = GetPlayer(gameContext);
            Player opponent = GetOpponent(gameContext);
            if (player.PickedCard is null)
            {
                throw new InvalidOperationException($"{player.Name} player's picked card is null, {nameof(BuildCard)} action cannot be performed!");
            }

            ICardNode card = player.PickedCard;
            GetComposition(gameContext).RemoveCard(card);
            player.Cards.Add(card.CardObj);
            int BuildCost = 0;
            bool chainBuildUsed = true;
            if (string.IsNullOrEmpty(card.CardObj.PreviousBuilding) ||
               player.Cards.All(c => c.Name != card.CardObj.PreviousBuilding))
            {
                BuildCost = gameContext.CostCalculator.GetBuildCost(card.CardObj, player, opponent);
                player.Money -= BuildCost;
                chainBuildUsed = false;
            }

            player.PickedCard = null;
            gameContext.EventManager.Publish(new OnCardBuilt(card.CardObj, player, BuildCost, chainBuildUsed));
            card.CardObj.OnBuilt(gameContext);

        }

        public bool CanPerform(IGameContext gameContext)
        {
            Player player = GetPlayer(gameContext);
            Player opponent = GetOpponent(gameContext);
            if (player.PickedCard is null)
            {
                return false;
            }

            Card card = player.PickedCard.CardObj;

            if (!string.IsNullOrEmpty(card.PreviousBuilding) &&
               player.Cards.Any(c => c.Name == card.PreviousBuilding))
                return true;


            return gameContext.CostCalculator.CanAfford(card, player, opponent);
        }

        private Player GetPlayer(IGameContext gameContext) => gameContext.TurnHandler.CurrentPlayer;
        private Player GetOpponent(IGameContext gameContext) => gameContext.TurnHandler.OpponentPlayer;

        private ICardComposition GetComposition(IGameContext gameContext) => gameContext.AgeHandler.CurrentAge.Composition;
    }
}
