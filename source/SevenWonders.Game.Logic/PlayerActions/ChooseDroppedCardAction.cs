using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.PlayerActions
{
    public class ChooseDroppedCardAction : IPlayerAction
    {
        public string Name => m_card.Name;
        public int Id => 7;
        public ChooseDroppedCardAction()
        {
            m_card = new RedCard();
        }

        public ChooseDroppedCardAction(Card card)
        {
            m_card = card;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return gameContext.DroppedCardList is not null && gameContext.DroppedCardList.Cards.Contains(m_card);
        }

        public bool DoPlayerAction(IGameContext gameContext)
        {
            if (gameContext.DroppedCardList is null)
            {
                throw new InvalidOperationException( $"{nameof(gameContext.DroppedCardList)} is not initialized! Cannot perform player action!");
            }
            ArgumentChecker.CheckPredicateForOperation(() => !gameContext.DroppedCardList.Cards.Contains(m_card), $"{nameof(gameContext.DroppedCardList)} does not contain the card! Cannot perform player action!");

            Player player = gameContext.TurnHandler.CurrentPlayer;
            Player opponent = gameContext.TurnHandler.OpponentPlayer;
            gameContext.DroppedCardList.Cards.Remove(m_card);
            player.Cards.Add(m_card);
            gameContext.EventManager.Publish(new OnObjectChosen(gameContext.DroppedCardList.Cards.Select(card => card.Name).ToArray(), true));
            OnCardBuilt onCardBuilt = new OnCardBuilt(m_card, player, 0, false);
            player.OnBuildCard(onCardBuilt);
            gameContext.EventManager.Publish(onCardBuilt);
            m_card.OnBuilt(gameContext, player, opponent);

            return true;
        }

        private readonly Card m_card;
    }
}
