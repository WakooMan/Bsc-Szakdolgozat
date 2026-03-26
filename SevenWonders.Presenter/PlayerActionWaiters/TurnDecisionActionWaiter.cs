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
            using var signal = new ManualResetEventSlim(false);

            void OnBuildCard() { chosenActionType = typeof(BuildCard); signal.Set(); }
            void OnSellCard() { chosenActionType = typeof(SellCard); signal.Set(); }
            void OnUnpickCard() { chosenActionType = typeof(UnpickCard); signal.Set(); }
            void OnBuildWonder() { chosenActionType = typeof(BuildWonder); signal.Set(); }

            m_cardPresenter.BuildCardChosen += OnBuildCard;
            m_cardPresenter.SellCardChosen += OnSellCard;
            m_cardPresenter.UnpickCardChosen += OnUnpickCard;
            m_cardPresenter.BuildWonderChosen += OnBuildWonder;

            signal.Wait();

            m_cardPresenter.BuildCardChosen -= OnBuildCard;
            m_cardPresenter.SellCardChosen -= OnSellCard;
            m_cardPresenter.UnpickCardChosen -= OnUnpickCard;
            m_cardPresenter.BuildWonderChosen -= OnBuildWonder;

            foreach (TurnDecision decision in playerActions)
            {
                if (decision.PlayerAction?.GetType() == chosenActionType)
                {
                    return decision;
                }
            }

            throw new InvalidOperationException($"No matching turn decision found for action type {chosenActionType?.Name}.");
        }

        private readonly ICardPresenter m_cardPresenter;
    }
}
