using SevenWonders.GameEngine;
using SevenWonders.Presenter.Views;
using System.Numerics;

namespace SevenWonders.Presenter.Presenters.Handlers
{
    public class PlayerCardHandler : IPlayerCardHandler
    {
        public PlayerCardHandler(IObjectManager objectManager, Scene scene, GraphicsLayer graphicsLayer, GameObject cardTarget)
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
                GameObject cardTarget = m_cardViews.Any() ? m_objectManager.CopyGameObject(m_scene, m_graphicsLayer, m_cardTarget, m_cardTarget.Name + m_cardViews.Count) : m_cardTarget;
                if (m_cardTarget != cardTarget)
                {
                    cardTarget.ZIndex--;
                    GameObject last = m_cardViews.Values.Last();
                    if (cardTarget.Rotation == 0f)
                    {
                        cardTarget.Position = last.Position - new Vector2(0f, last.Height/4);
                    }
                    else
                    {
                        cardTarget.Position = last.Position + new Vector2(0f, last.Height/4);
                    }
                }
   
                var group = cardView.GetAnimationGroupBuilder()
                    .MoveTo(cardTarget, 0.5f)
                    .Unhighlight(false, 0.5f);
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
