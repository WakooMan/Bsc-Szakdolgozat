using SevenWonders.Game.Logic.Elements.Military;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Presenter.Connectors;
using SevenWonders.Game.Presenter.Connectors.MilitaryBoard;
using SevenWonders.Game.Presenter.Views;
using SevenWonders.Game.Presenter.Views.Factories;
using System;
using SevenWonders.Game.Engine.SceneObjects;

namespace SevenWonders.Game.Presenter.Presenters
{
    public class MilitaryBoardPresenter : IPresenter
    {
        public MilitaryBoardPresenter(IGameEngineReceiver gameEngineReceiver, IEventManager eventManager, IGameObjectViewFactory gameObjectViewFactory, ITextureIdHandler textureIdHandler, IMilitaryTokenChildTextureHandler militaryTokenChildTextureHandler)
        {
            m_eventManager = eventManager;
            m_gameEngineReceiver = gameEngineReceiver;
            m_gameObjectViewFactory = gameObjectViewFactory;
            m_militaryObjects = new List<GameObject>();
            m_player1ScientificObjects = new List<(GameObject science, TextLabel label)>();
            m_player2ScientificObjects = new List<(GameObject science, TextLabel label)>();
            m_militaryTokenChildTextureHandler = militaryTokenChildTextureHandler;
            m_textureIdHandler = textureIdHandler;
        }

        public void Initialize()
        {
            m_militaryObjects.AddRange(m_gameEngineReceiver.ReceiveGameObjects("military", 19));
            var player1Science = m_gameEngineReceiver.ReceiveGameObjects("player1science", 6).ToList();
            var player1ScienceLabel = m_gameEngineReceiver.ReceiveTextLabels("player1sciencelabel", 6).ToList();

            for (int i = 0; i < 6; i++)
            {
                m_player1ScientificObjects.Add((player1Science[i], player1ScienceLabel[i]));
            }

            var player2Science = m_gameEngineReceiver.ReceiveGameObjects("player2science", 6).ToList();
            var player2ScienceLabel = m_gameEngineReceiver.ReceiveTextLabels("player2sciencelabel", 6).ToList();

            for (int i = 0; i < 6; i++)
            {
                m_player2ScientificObjects.Add((player2Science[i], player2ScienceLabel[i]));
            }

            m_military = m_gameObjectViewFactory.CreateView("military");
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnMilitaryBoardChanged>(OnMilitaryBoardChanged);
            m_eventManager.Subscribe<OnPlayerUpdate>(OnPlayerUpdate);
            m_eventManager.Subscribe<OnMilitaryTokenReachedThreshold>(OnMilitaryTokenReachedThreshold);
            m_eventManager.Subscribe<OnGameInitialized>(OnGameInitialized);
        }

        private void OnGameInitialized(OnGameInitialized initialized)
        {
            foreach (MilitaryCard militaryCard in initialized.GameContext.MilitaryBoard.MilitaryCards)
            {
                m_militaryTokenChildTextureHandler.Handle(militaryCard);
            }
        }

        private void OnMilitaryTokenReachedThreshold(OnMilitaryTokenReachedThreshold threshold)
        {
            foreach (MilitaryCard militaryCard in threshold.MilitaryCards)
            {
                GameObject gameObject = m_gameEngineReceiver.ReceiveGameObject(militaryCard.Name);
                if (gameObject is not null)
                {
                    int spriteIdx = gameObject.Animations.FindIndex(anim => anim.Name == "Back");
                    if (spriteIdx != -1)
                    {
                        gameObject.CurrentAnim = spriteIdx;
                    }
                }
            }
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

        private void OnPlayerUpdate(OnPlayerUpdate progress)
        {
            UpdateScientificState(progress.Player1.Disciplines, progress.Player1.Owner.Id);
            UpdateScientificState(progress.Player2.Disciplines, progress.Player2.Owner.Id);
        }

        private void UpdateScientificState(IReadOnlyDictionary<Type, int> disciplines, int playerId)
        {
            int i = 0;
            foreach (var discipline in disciplines)
            {
                if (playerId == 1)
                {
                    MakeScientificProgressVisible(m_player1ScientificObjects[i], discipline.Key, discipline.Value);
                }
                else
                {
                    MakeScientificProgressVisible(m_player2ScientificObjects[i], discipline.Key, discipline.Value);
                }
                i++;
            }
        }

        private void MakeScientificProgressVisible((GameObject science, TextLabel label) pair, Type type, int Number)
        {
            var anim = pair.science.Animations.FirstOrDefault();
            if (anim is not null)
            {
                var frame = anim.Frames.FirstOrDefault();
                if (frame is not null && frame.TextureId != m_textureIdHandler.GetTextureId(type.Name))
                {
                    frame.TextureId = m_textureIdHandler.GetTextureId(type.Name);
                    pair.label.TextProperties.Text = Number.ToString();
                    pair.science.Visible = true;
                    pair.label.Visible = true;
                }
            }
        }

        private IGameObjectView? m_military;
        private readonly List<GameObject> m_militaryObjects;
        private readonly List<(GameObject science, TextLabel label)> m_player1ScientificObjects;
        private readonly List<(GameObject science, TextLabel label)> m_player2ScientificObjects;
        private readonly IEventManager m_eventManager;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly ITextureIdHandler m_textureIdHandler;
        private readonly IMilitaryTokenChildTextureHandler m_militaryTokenChildTextureHandler;
    }
}
