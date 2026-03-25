using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using GameLogic.PlayerActions;
using SevenWonders.Presenter.PlayerActionHandler;
using SevenWonders.Presenter.Presenters;

namespace SevenWonders.Presenter.PlayerActionWaiters
{
    public class PickCardActionWaiter : IPlayerActionWaiter<PickCard>
    {
        public PickCardActionWaiter(ICardPresenter cardPresenter)
        {
            m_cardPresenter = cardPresenter;
        }
        public PickCard WaitForPlayerAction(ICollection<PickCard> playerActions)
        {
            PickCard? result = null;
            while(result is null)
            {
                Card? card = null;
                m_cardPresenter.CardChosen += (c) => {
                    card = c;
                };
                while (card is null)
                {
                    Thread.Sleep(100);
                }

                foreach (PickCard action in playerActions)
                {
                    if (action.CardNode.CardObj.Name == card.Name)
                    {
                        m_cardPresenter.MoveToPlayer(action.Player, card);
                        result = action;
                    }
                }
            }

            return result;
        }

        private readonly ICardPresenter m_cardPresenter;
    }
}
