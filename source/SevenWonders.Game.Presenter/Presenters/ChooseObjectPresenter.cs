using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Engine;
using SevenWonders.Game.Presenter.Connectors;
using SevenWonders.Game.Presenter.Views;
using SevenWonders.Game.Presenter.Views.Factories;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.Game.Presenter.Presenters
{
    public class ChooseObjectPresenter: IPresenter
    {

        public ChooseObjectPresenter(IGameEngineReceiver gameEngineReceiver, IEventManager eventManager, IGameObjectViewFactory gameObjectViewFactory, IObjectManager objectManager)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_gameObjectViewFactory = gameObjectViewFactory;
            m_objectCache = new List<(IGameObjectView objectView, GameObject previousPosTarget)>();
            m_objectManager = objectManager;
            m_currentObject = -1;
        }

        public void Initialize()
        {
            m_chooseObjectLayer = m_gameEngineReceiver.ReceiveGraphicsLayer("ChooseObjectLayer");
            m_chooseObjectTitle = m_gameEngineReceiver.ReceiveTextLabel("ChooseObjectTitle");
            m_centerTarget = m_gameEngineReceiver.ReceiveGameObject("ChooseObjectCenterTarget");
            m_previousElement = m_gameEngineReceiver.ReceiveGameObject("ChooseObjectPreviousElement");
            m_nextElement = m_gameEngineReceiver.ReceiveGameObject("ChooseObjectNextElement");
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnChooseObjects>(eventObj =>
            {
                lock (m_objectCache)
                {
                    if (m_objectCache.Count > 0)
                    {
                        throw new InvalidOperationException("The object view cache is not cleared before publishing this event!");
                    }
                    m_chooseObjectTitle.Text = eventObj.Title;
                    foreach (string objectName in eventObj.Objects)
                    {
                        GameObject gameObject = m_gameEngineReceiver.ReceiveGameObject(objectName);
                        IGameObjectView gameObjectView = m_gameObjectViewFactory.CreateView(objectName);
                        GameObject previousPosTarget = new GameObject()
                        {
                            Name = gameObject.Name + "previousPositionTarget",
                            Visible = false,
                            Rotation = gameObject.Rotation,
                            Position = gameObject.Position,
                            ZIndex = gameObject.ZIndex,
                        };
                        m_objectManager.AddSceneObject(m_chooseObjectLayer, previousPosTarget);
                        m_objectCache.Add((gameObjectView, previousPosTarget));
                        int frontSpriteIdx = gameObjectView.FindAnimationIndexByName("front");
                        var group = gameObjectView.GetAnimationGroupBuilder().MoveTo(m_centerTarget, 0.5f).Highlight(m_centerTarget.VisualSize, false, 0.5f);
                        if (frontSpriteIdx >= 0)
                        {
                            group.Flip("front", 0.5f);
                        }
                        gameObjectView.Execute().GetAwaiter().GetResult();
                        gameObjectView.SetVisible(true);
                    }
                    m_currentObject = 0;
                    UpdateProperties();
                    m_previousElement.ClickedEvent += OnPreviousElementClicked;
                    m_nextElement.ClickedEvent += OnNextElementClicked;
                    m_chooseObjectLayer.Visible = true;
                }
            });

            m_eventManager.Subscribe<OnObjectChosen>(eventObj =>
            {
                lock (m_objectCache)
                {
                    m_previousElement.ClickedEvent -= OnPreviousElementClicked;
                    m_nextElement.ClickedEvent -= OnNextElementClicked;
                    foreach (var cache in m_objectCache)
                    {
                        IGameObjectView gameObjectView = cache.objectView;
                        if (eventObj.Objects.Contains(gameObjectView.Name))
                        {
                            gameObjectView.SetVisible(eventObj.Visible);
                            int backSpriteIdx = gameObjectView.FindAnimationIndexByName("back");
                            var group = gameObjectView.GetAnimationGroupBuilder().MoveTo(cache.previousPosTarget, 0.5f).Highlight(Vector2.One, false, eventObj.Visible ? 0.5f : 0f);
                            if (!eventObj.Visible && backSpriteIdx >= 0)
                            {
                                group.Flip("back", 0.5f);
                            }
                            gameObjectView.Execute().GetAwaiter().GetResult();
                            m_objectManager.RemoveSceneObject(m_chooseObjectLayer, cache.previousPosTarget);
                        }
                    }
                    m_chooseObjectLayer.Visible = false;
                    m_objectCache.Clear();
                    m_currentObject = -1;
                }
            });
        }

        private bool CanClickNextElement()
        {
            return m_currentObject < m_objectCache.Count - 1;
        }

        private bool CanClickPreviousElement()
        {
            return 0 < m_currentObject && 0 < m_objectCache.Count;
        }

        private void OnNextElementClicked(IInteractiveObject interactiveObject, SKTouchEventArgs eventArgs)
        {
            if (CanClickNextElement())
            {
                m_currentObject++;
                UpdateProperties();
            }
        }

        private void OnPreviousElementClicked(IInteractiveObject interactiveObject, SKTouchEventArgs eventArgs)
        {
            if (CanClickPreviousElement())
            {
                m_currentObject--;
                UpdateProperties();
            }
        }

        private void UpdateProperties()
        {
            for (int i = 0; i < m_objectCache.Count; i++)
            {
                m_objectCache[i].objectView.SetVisible((m_currentObject == i) ? true : false);
            }

            m_previousElement.Dimmed = !CanClickPreviousElement();
            m_nextElement.Dimmed = !CanClickNextElement();
        }

        private GraphicsLayer? m_chooseObjectLayer;
        private TextLabel? m_chooseObjectTitle;
        private GameObject? m_centerTarget;
        private GameObject? m_previousElement;
        private GameObject? m_nextElement;
        private int m_currentObject;
        private readonly IEventManager m_eventManager;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly IObjectManager m_objectManager;
        private readonly List<(IGameObjectView objectView, GameObject previousPosTarget)> m_objectCache;
    }
}
