using GameLogic.Elements.Military;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWonders.Presenter.Presenters
{
    public class MilitaryBoardPresenter : IPresenter
    {
        public MilitaryBoardPresenter(IGameEngineReceiver gameEngineReceiver, IEventManager eventManager, IGameObjectViewFactory gameObjectViewFactory)
        {
            m_eventManager = eventManager;
            m_gameEngineReceiver = gameEngineReceiver;
            m_gameObjectViewFactory = gameObjectViewFactory;
            m_militaryObjects = new List<GameObject>();
            m_player1ScientificObjects = new List<GameObject>();
            m_player2ScientificObjects = new List<GameObject>();
        }

        public void Initialize()
        {
            m_militaryObjects.AddRange(m_gameEngineReceiver.ReceiveGameObjects("military", 19));
            m_player1ScientificObjects.AddRange(m_gameEngineReceiver.ReceiveGameObjects("player1science", 6));
            m_player2ScientificObjects.AddRange(m_gameEngineReceiver.ReceiveGameObjects("player2science", 6));
            m_military = m_gameObjectViewFactory.CreateView("military");
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnMilitaryBoardChanged>(OnMilitaryBoardChanged);
            m_eventManager.Subscribe<OnScientificProgress>(OnScientificProgress);
        }

        private void OnMilitaryBoardChanged(OnMilitaryBoardChanged advanced)
        {
            int index = advanced.Fields.IndexOf(MilitaryField.Shield);
            if(m_military is not null && index >= 0 && index < m_militaryObjects.Count)
            {
                m_military.GetAnimationGroupBuilder().MoveTo(m_militaryObjects[index], 0.5f);
                m_military.Execute().GetAwaiter().GetResult();
            }
        }

        private void OnScientificProgress(OnScientificProgress progress)
        {
            var disciplines = progress.Player.Disciplines.Keys.ToList();
            for (int i = 0; i < disciplines.Count; i++)
            {
                if (progress.Player.Id == 1)
                {
                    MakeScientificProgressVisible(m_player1ScientificObjects[i], disciplines[i]);
                }
                else
                {
                    MakeScientificProgressVisible(m_player2ScientificObjects[i], disciplines[i]);
                }
            }
        }

        private void MakeScientificProgressVisible(GameObject gameObject, Type type)
        {
            var anim = gameObject.Animations.FirstOrDefault();
            if (anim is not null)
            {
                var frame = anim.Frames.FirstOrDefault();
                if (frame is not null && frame.TextureId != TextureIdDictionary.GetTextureId(type.Name))
                {
                    frame.TextureId = TextureIdDictionary.GetTextureId(type.Name);
                    gameObject.Visible = true;
                }
            }
        }

        private IGameObjectView? m_military;
        private readonly List<GameObject> m_militaryObjects;
        private readonly List<GameObject> m_player1ScientificObjects;
        private readonly List<GameObject> m_player2ScientificObjects;
        private readonly IEventManager m_eventManager;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
    }
}
