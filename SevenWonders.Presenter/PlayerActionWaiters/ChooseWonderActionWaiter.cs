using GameLogic.Elements.Wonders;
using GameLogic.PlayerActions;
using SevenWonders.Presenter.PlayerActionHandler;
using SevenWonders.Presenter.Presenters;

namespace SevenWonders.Presenter.PlayerActionWaiters
{
    public class ChooseWonderActionWaiter: IPlayerActionWaiter<ChooseWonderAction>
    {
        public ChooseWonderActionWaiter(IWonderPresenter wonderPresenter)
        {
            m_wonderPresenter = wonderPresenter;
        }

        public ChooseWonderAction WaitForPlayerAction(ICollection<ChooseWonderAction> playerActions)
        {
            Wonder? wonder = null;
            using var signal = new ManualResetEventSlim(false);
            m_wonderPresenter.WonderChosen += (w) => {
                wonder = w; signal.Set();
            };

            while (wonder is null)
            {
                signal.Wait();

                if (wonder is not null)
                {
                    foreach (ChooseWonderAction action in playerActions)
                    {
                        if (action.Wonder.Name == wonder.Name)
                        {
                            m_wonderPresenter.MoveToPlayer(action.Player, wonder);
                            return action;
                        }
                    }
                    wonder = null;
                }
            }

            throw new InvalidOperationException($"No matching wonder action.");
        }

        private readonly IWonderPresenter m_wonderPresenter;
    }
}
