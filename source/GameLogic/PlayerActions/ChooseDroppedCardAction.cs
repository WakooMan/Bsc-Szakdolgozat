using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class ChooseDroppedCardAction : IPlayerAction
    {
        public string Name => m_card.Name;
        public ChooseDroppedCardAction()
        {
            m_card = new RedCard();
        }

        public ChooseDroppedCardAction(Card card)
        {
            m_card = card;
        }

        public Task<bool> CanPerform(IGameContext gameContext)
        {
            return Task.FromResult(gameContext.DroppedCardList is not null && gameContext.DroppedCardList.Cards.Contains(m_card));
        }

        public async Task<bool> DoPlayerAction(IGameContext gameContext)
        {
            if (gameContext.DroppedCardList is null)
            {
                throw new InvalidOperationException( $"{nameof(gameContext.DroppedCardList)} is not initialized! Cannot perform player action!");
            }
            ArgumentChecker.CheckPredicateForOperation(() => !gameContext.DroppedCardList.Cards.Contains(m_card), $"{nameof(gameContext.DroppedCardList)} does not contain the card! Cannot perform player action!");

            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.DroppedCardList.Cards.Remove(m_card);
            player.Cards.Add(m_card);
            await gameContext.EventManager.PublishAsync(new OnObjectChosen(gameContext.DroppedCardList.Cards.Select(card => card.Name).ToArray()));
            await gameContext.EventManager.PublishAsync(new OnCardBuilt(m_card, player, 0, false));
            await m_card.OnBuilt(gameContext);

            return true;
        }

        private readonly Card m_card;
    }
}
