using GameLogic.Elements;
using GameLogic.Elements.Wonders;

namespace SevenWonders.Presenter.Presenters
{
    public interface IWonderPresenter
    {
        delegate void WonderPresenterDelegate(Wonder wonder);
        event WonderPresenterDelegate WonderChosen;
        void MoveToPlayer(Player player, Wonder wonder);
        void MoveToCenter(Wonder wonder);
        void MoveToDeck(Wonder wonder);
        void Initialize();
    }
}
