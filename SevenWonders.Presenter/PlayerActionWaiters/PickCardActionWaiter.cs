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
            Card? card = null;
            using var signal = new ManualResetEventSlim(false);
            m_cardPresenter.CardChosen += (c) => {
                card = c; signal.Set();
            };

            while (card is null)
            {
                signal.Wait();

                if (card is not null)
                {
                    foreach (PickCard action in playerActions)
                    {
                        if (action.CardNode.CardObj.Name == card.Name)
                        {
                            return action;
                        }
                    }
                    card = null;
                }
            }

            throw new InvalidOperationException($"No matching pick card action.");
        }

        private readonly ICardPresenter m_cardPresenter;
    }
}
