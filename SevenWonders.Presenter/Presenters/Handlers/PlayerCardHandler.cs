using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Presenter.Presenters.Handlers
{
    public class PlayerCardHandler
    {

        public PlayerCardHandler(IObjectManager objectManager, GameObject cardTarget, Scene scene, GraphicsLayer graphicsLayer)
        {
            m_objectManager = objectManager;
            m_cardTarget = cardTarget;
            m_scene = scene;
            m_graphicsLayer = graphicsLayer;
            m_cardViews = new Dictionary<IGameObjectView, GameObject>();
        }

        public async Task MoveCardToTarget(IGameObjectView cardView)
        {
            if(!m_cardViews.ContainsKey(cardView))
            {
                GameObject cardTarget = m_objectManager.CopyGameObject(m_scene, m_graphicsLayer, m_cardTarget, m_cardTarget.Name + m_cardViews.Count);
                var group = cardView.GetAnimationGroupBuilder().MoveTo(cardTarget, 0.5f);
                await cardView.Execute();
                m_cardViews.Add(cardView, cardTarget);
            }
        }

        private readonly Dictionary<IGameObjectView, GameObject> m_cardViews;
        private readonly IObjectManager m_objectManager;
        private readonly GameObject m_cardTarget;
        private readonly Scene m_scene;
        private readonly GraphicsLayer m_graphicsLayer;
    }
}
