using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class BuildCard : IPlayerAction
    {
        public BuildCard(IGameContext gameContext)
        {
            m_gameContext = gameContext;
        }

        public void DoPlayerAction()
        {
            if (Player.PickedCard is null)
            {
                throw new InvalidOperationException($"{Player.Name} player's picked card is null, {nameof(BuildCard)} action cannot be performed!");
            }

            ICardNode card = Player.PickedCard;
            Composition.RemoveCard(card);
            Player.Cards.Add(card.CardObj);
            int BuildCost = 0;
            bool chainBuildUsed = true;
            if (string.IsNullOrEmpty(card.CardObj.PreviousBuilding) ||
               Player.Cards.All(c => c.Name != card.CardObj.PreviousBuilding))
            {
                BuildCost = m_gameContext.CostCalculator.GetBuildCost(card.CardObj, Player, Opponent);
                Player.Money -= BuildCost + card.CardObj.MoneyCost;
                chainBuildUsed = false;
            }

            Player.PickedCard = null;
            m_gameContext.EventManager.Publish(GameEventType.CardBuilt, new OnCardBuilt(card.CardObj, Player, BuildCost, chainBuildUsed));
            card.CardObj.OnBuilt(m_gameContext);

        }

        public bool CanPerform()
        {
            if (Player.PickedCard is null)
            {
                return false;
            }

            Card card = Player.PickedCard.CardObj;

            if (!string.IsNullOrEmpty(card.PreviousBuilding) &&
               Player.Cards.Any(c => c.Name == card.PreviousBuilding))
                return true;


            return m_gameContext.CostCalculator.CanAfford(card, Player, Opponent);
        }

        private Player Player => m_gameContext.TurnHandler.CurrentPlayer;
        private Player Opponent => m_gameContext.TurnHandler.OpponentPlayer;

        private ICardComposition Composition => m_gameContext.AgeHandler.CurrentAge.Composition;
        private readonly IGameContext m_gameContext;
    }
}
