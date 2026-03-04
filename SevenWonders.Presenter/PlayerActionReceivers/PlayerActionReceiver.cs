using GameLogic.Elements;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.Presenter.PlayerActionHandler;

namespace SevenWonders.Presenter.PlayerActionReceivers
{
    public class PlayerActionReceiver : IPlayerActionReceiver
    {
        public PlayerActionReceiver(IServiceProvider serviceProvider)
        {
            m_serviceProvider = serviceProvider;
        }
        public TPlayerAction ReceivePlayerAction<TPlayerAction>(Player player, ICollection<TPlayerAction> playerActions) where TPlayerAction : class, IPlayerAction, new()
        {
            var waiter = m_serviceProvider.GetService<IPlayerActionWaiter<TPlayerAction>>();

            if (waiter == null)
            {
                throw new InvalidOperationException($"There is no registered waiter for type: {typeof(TPlayerAction).Name}");
            }

            return waiter.WaitForPlayerAction(playerActions);
        }

        private readonly IServiceProvider m_serviceProvider;
    }
}
