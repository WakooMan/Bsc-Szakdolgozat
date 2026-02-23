using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.Views;

namespace SevenWondersUI.Views
{
    public class WonderView : IWonderView
    {
        public WonderView(GameObject wonder, ICardFlipComponent cardFlipComponent, IMoverComponent moverComponent)
        {
            m_wonder = wonder;
            m_flipComponent = cardFlipComponent;
            m_moverComponent = moverComponent;
        }

        public void MoveTo(GameObject target)
        {
            m_moverComponent.MoveTo(m_wonder, target, 210, 30);
            m_flipComponent.Flip(m_wonder, 0, 0.6f);
        }

        public void Highlight()
        {
            throw new NotImplementedException();
        }

        public void Lift()
        {
            throw new NotImplementedException();
        }

        private readonly ICardFlipComponent m_flipComponent;
        private readonly IMoverComponent m_moverComponent;
        private readonly GameObject m_wonder;
    }
}
