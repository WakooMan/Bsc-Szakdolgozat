using GameLogic;
using SevenWonders.AI.Model.Cache;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.PlayerActionReceivers;
using SevenWonders.Presenter.Presenters;
using SevenWondersUI.Services;

namespace SevenWondersUI.ViewModels
{
    public class PlayerVSAIGamePageViewModel : BaseGamePageViewModel
    {
        protected override RandomGeneratorType RandomGeneratorType => RandomGeneratorType.Undeterministic;

        protected override PlayerType Player1Type => PlayerType.LocalPlayer;

        protected override PlayerType Player2Type => m_player2Type;

        public PlayerVSAIGamePageViewModel(IGame game,
                                           IEngine engine,
                                           ISceneLoader sceneLoader,
                                           IAnimationManager animationManager,
                                           IPresenterStore presenterStore,
                                           IPlayerActionReceiverFactory playerActionReceiverFactory,
                                           IRandomGeneratorFactory randomGeneratorFactory,
                                           INavigationService navigationService,
                                           IAIDecisionHandlerCache aIDecisionHandlerCache) : base(game, engine, sceneLoader, animationManager, presenterStore, playerActionReceiverFactory, randomGeneratorFactory)
        {
            m_navigationService = navigationService;
            m_aIDecisionHandlerCache = aIDecisionHandlerCache;
            m_player2Type = PlayerType.EasyAI;
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            base.ApplyQueryAttributes(query);
            if (query.TryGetValue("Player2Type", out object? obj) && obj is PlayerType player2Type)
            {
                m_player2Type = player2Type;
            }
        }

        protected override void InitializeGame()
        {
            base.InitializeGame();
            switch (m_player2Type)
            {
                case PlayerType.EasyAI:
                    m_aIDecisionHandlerCache.EasyAI.Initialize(2);
                    break;
                case PlayerType.MediumAI:
                    m_aIDecisionHandlerCache.MediumAI.Initialize(2);
                    break;
                case PlayerType.HardAI:
                    m_aIDecisionHandlerCache.HardAI.Initialize(2);
                    break;
                default:
                    m_aIDecisionHandlerCache.EasyAI.Initialize(2);
                    break;
            }
        }


        public override async Task OnGameOver()
        {
            await m_navigationService.NavigateToAsync("//MainPage");
        }

        private readonly INavigationService m_navigationService;
        private readonly IAIDecisionHandlerCache m_aIDecisionHandlerCache;
        private PlayerType m_player2Type;
    }
}
