using GameLogic.Elements.Wonders;

namespace SevenWonders.Presenter.Presenters
{
    public interface IWonderPresenter
    {
        void MoveToPlayer1(Wonder wonder);
        void MoveToPlayer2(Wonder wonder);
        void MoveToCenter(Wonder wonder);
        void Initialize();
    }
}
