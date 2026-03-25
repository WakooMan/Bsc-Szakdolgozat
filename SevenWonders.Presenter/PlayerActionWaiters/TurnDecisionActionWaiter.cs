using GameLogic.PlayerActions;
using SevenWonders.Presenter.PlayerActionHandler;
using SevenWonders.Presenter.Presenters;

namespace SevenWonders.Presenter.PlayerActionWaiters
{
    public class TurnDecisionActionWaiter : IPlayerActionWaiter<TurnDecision>
    {
        public TurnDecisionActionWaiter(ICardPresenter cardPresenter)
        {
            m_cardPresenter = cardPresenter;
        }

        public TurnDecision WaitForPlayerAction(ICollection<TurnDecision> playerActions)
        {
            Type? chosenActionType = null;

            m_cardPresenter.BuildCardChosen += () => chosenActionType = typeof(BuildCard);
            m_cardPresenter.SellCardChosen += () => chosenActionType = typeof(SellCard);
            m_cardPresenter.UnpickCardChosen += () => chosenActionType = typeof(UnpickCard);
            m_cardPresenter.BuildWonderChosen += () => chosenActionType = typeof(BuildWonder);

            while (chosenActionType is null)
            {
                Thread.Sleep(100);
            }

            foreach (TurnDecision decision in playerActions)
            {
                if (decision.PlayerAction?.GetType() == chosenActionType)
                {
                    return decision;
                }
            }

            throw new InvalidOperationException($"No matching turn decision found for action type {chosenActionType.Name}.");
        }

        private readonly ICardPresenter m_cardPresenter;
    }
}
