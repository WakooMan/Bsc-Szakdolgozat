using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class ChooseCardAction : IPlayerAction
    {
        public ChooseCardAction(Card card)
        {
            m_card = card;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return gameContext.DroppedCardList is not null && gameContext.DroppedCardList.Cards.Contains(m_card);
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            if (gameContext.DroppedCardList is null)
            {
                throw new InvalidOperationException( $"{nameof(gameContext.DroppedCardList)} is not initialized! Cannot perform player action!");
            }
            ArgumentChecker.CheckPredicateForOperation(() => !gameContext.DroppedCardList.Cards.Contains(m_card), $"{nameof(gameContext.DroppedCardList)} does not contain the card! Cannot perform player action!");

            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.DroppedCardList.Cards.Remove(m_card);
            player.Cards.Add(m_card);
            gameContext.EventManager.Publish(new OnCardBuilt(m_card, player, 0, false));
            m_card.OnBuilt(gameContext);
        }

        private readonly Card m_card;
    }
}
