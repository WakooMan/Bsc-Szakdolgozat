using GameLogic.Elements.Wonders;
using SevenWonders.GameEngine;

namespace SevenWonders.Presenter
{
    public class WonderPresenter
    {
        public WonderPresenter(IWonderView wonderView, Wonder wonder)
        {
            m_wonderView = wonderView;
            m_wonder = wonder;
        }

        public void MoveToPlayer()
        {

        }

        public void MoveToCenter()
        {

        }

        private IWonderView m_wonderView;
        private Wonder m_wonder;
    }
}
