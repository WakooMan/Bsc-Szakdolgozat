using SevenWonders.GameEngine;
using SevenWonders.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SevenWondersUI.Views
{
    public class WonderView : IWonderView
    {
        public WonderView(GameObject wonder)
        {
            m_wonder = wonder;
        }

        private readonly GameObject m_wonder;

        public void MoveGameObjectTo(GameObject gameObject, Vector2 target)
        {
            throw new NotImplementedException();
        }

        public void HighlightGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public void LiftGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}
