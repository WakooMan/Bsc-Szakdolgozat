using SevenWonders.Game.Engine;
using SevenWonders.Game.Engine.SceneHandling;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Presenter.Views;
using System.Numerics;

namespace SevenWonders.Game.Presenter.Presenters.Handlers
{
    public class DevelopmentHandler : IDevelopmentHandler
    {
        public DevelopmentHandler(IObjectManager objectManager, GraphicsLayer graphicsLayer, GameObject developmentTarget)
        {
            m_objectManager = objectManager;
            m_developmentViews = new Dictionary<IGameObjectView, GameObject>();
            m_developmentTarget = developmentTarget;
            m_graphicsLayer = graphicsLayer;
        }

        public async Task MoveDevelopmentToTarget(IGameObjectView developmentView)
        {
            if (!m_developmentViews.ContainsKey(developmentView))
            {
                GameObject developmentTarget = m_developmentViews.Any() ? m_objectManager.CopyGameObject(m_graphicsLayer, m_developmentTarget, m_developmentTarget.Name + m_developmentViews.Count) : m_developmentTarget;
                if (m_developmentTarget != developmentTarget)
                {
                    GameObject last = m_developmentViews.Values.Last();
                    if (developmentTarget.Rotation == 0f)
                    {
                        developmentTarget.Position = last.Position - new Vector2(0f, last.Height + last.Height / 4);
                    }
                    else
                    {
                        developmentTarget.Position = last.Position + new Vector2(0f, last.Height + last.Height / 4);
                    }
                }

                developmentView.SetVisible(true);
                var group = developmentView.GetAnimationGroupBuilder()
                    .MoveTo(developmentTarget, 0.5f)
                    .Unhighlight(false, 0.5f);
                await developmentView.Execute();
                m_developmentViews.Add(developmentView, developmentTarget);
            }
        }

        private readonly Dictionary<IGameObjectView, GameObject> m_developmentViews;
        private readonly IObjectManager m_objectManager;
        private readonly GameObject m_developmentTarget;
        private readonly GraphicsLayer m_graphicsLayer;
    }
}
