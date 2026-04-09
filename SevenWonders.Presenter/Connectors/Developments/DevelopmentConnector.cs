using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.Effects;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors.Wonders;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Presenter.Connectors.Developments
{
    public class DevelopmentConnector : IDevelopmentConnector
    {
        public DevelopmentConnector(IGameElements gameElements, IGameObjectViewFactory gameObjectViewFactory, IGameEngineReceiver gameEngineReceiver)
        {
            m_developmentList = gameElements.Developments;
            m_gameObjectViewFactory = gameObjectViewFactory;
            m_gameEngineReceiver = gameEngineReceiver;
        }
        public IDictionary<Development, IGameObjectView> ReceiveDevelopmentConnection()
        {
            Dictionary<Development, IGameObjectView> result = new Dictionary<Development, IGameObjectView>();
            foreach (Development development in m_developmentList.Developments)
            {
                HandleDevelopment(development);
                result.Add(development, m_gameObjectViewFactory.CreateView(development.Name));
            }

            return result;
        }

        private void HandleDevelopment(Development development)
        {
            GameObject gameObject = m_gameEngineReceiver.ReceiveGameObject(development.Name);
            if (gameObject is not null)
            {
                Sprite? sprite = gameObject.Animations.FirstOrDefault(s => s.Name == "Front");
                if (sprite is not null)
                {
                    sprite.AddChildObject(new ChildTextLabel
                    {
                        TextLabel = new TextLabel()
                        {
                            BackgroundTextureId = -1,
                            Text = development.Name,
                            TextColor = SKColors.AntiqueWhite,
                            FontSize = 8,
                            Visible = true,
                        },
                        WidthPercent = 0.7f,
                        HeightPercent = 0.1f,
                        PositionPercent = new Vector2(0.15f, 0.7f)
                    });
                }
            }
        }

        private readonly IDevelopmentList m_developmentList;
        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
    }
}
