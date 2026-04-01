using GameLogic.Elements;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class UnpickCard : IPlayerAction
    {
        public string Name => nameof(UnpickCard);
        public UnpickCard(Player player)
        {
            ArgumentChecker.CheckNull(player, nameof(player));

            m_player = player;
        }

        public Task<bool> CanPerform(IGameContext gameContext)
        {
            return Task.FromResult(m_player.PickedCard is not null);
        }

        public async Task<bool> DoPlayerAction(IGameContext gameContext)
        {
            if (m_player.PickedCard is null)
            {
                throw new InvalidOperationException("Cannot perform action if picked card is null!");
            }

            ICardNode cardNode = m_player.PickedCard;
            m_player.PickedCard = null;
            await gameContext.EventManager.PublishAsync(new OnCardUnpicked(m_player, cardNode));
            return true;
        }

        private readonly Player m_player;
    }
}
