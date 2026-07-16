using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Developments;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Engine;
using SevenWonders.Game.Presenter.Connectors.Wonders;
using SevenWonders.Game.Presenter.Views;
using SevenWonders.Game.Presenter.Views.Factories;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SevenWonders.Game.Engine.SceneObjects;

namespace SevenWonders.Game.Presenter.Connectors.Developments
{
    public class DevelopmentConnector : IDevelopmentConnector
    {
        public DevelopmentConnector(IGameElements gameElements, IGameObjectViewFactory gameObjectViewFactory, IGameEngineReceiver gameEngineReceiver)
        {
            m_gameElements = gameElements;
            m_gameObjectViewFactory = gameObjectViewFactory;
            m_gameEngineReceiver = gameEngineReceiver;
        }
        public IDictionary<Development, IGameObjectView> ReceiveDevelopmentConnection()
        {
            IDevelopmentList? developmentList = m_gameElements.Developments;
            Dictionary<Development, IGameObjectView> result = new Dictionary<Development, IGameObjectView>();
            if (developmentList is null)
            {
                return result;
            }

            foreach (Development development in developmentList.Developments)
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

        private readonly IGameElements m_gameElements;
        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
    }
}
