using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class UnpickCard : IPlayerAction
    {
        public UnpickCard(Player player)
        {
            ArgumentChecker.CheckNull(player, nameof(player));

            m_player = player;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return m_player.PickedCard is not null;
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            if (m_player.PickedCard is null)
            {
                throw new InvalidOperationException("Cannot perform action if picked card is null!");
            }

            ICardNode cardNode = m_player.PickedCard;
            m_player.PickedCard = null;
            gameContext.EventManager.Publish(new OnCardUnpicked(m_player, cardNode));
        }

        private readonly Player m_player;
    }
}
