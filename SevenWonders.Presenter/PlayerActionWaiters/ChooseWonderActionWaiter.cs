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
            m_wonderPresenter.WonderChosen += (w) => {
                wonder = w;
            };
            while (wonder is null) {
                Thread.Sleep(100);
            }

            foreach (ChooseWonderAction action in playerActions)
            {
                if (action.Wonder.Name == wonder.Name)
                {
                    m_wonderPresenter.MoveToPlayer(action.Player, wonder);
                    return action;
                }
            }

            throw new InvalidOperationException("Chosen wonder is not among the choosable wonders!");
        }

        private readonly IWonderPresenter m_wonderPresenter;
    }
}
