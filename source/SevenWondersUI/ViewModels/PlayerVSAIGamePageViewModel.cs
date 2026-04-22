using GameLogic;
using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;
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

        protected override PlayerType Player2Type => PlayerType.AI;

        public PlayerVSAIGamePageViewModel(IGame game,
                                           IEngine engine,
                                           ISceneLoader sceneLoader,
                                           IAnimationManager animationManager,
                                           IPresenterStore presenterStore,
                                           IPlayerActionReceiverFactory playerActionReceiverFactory,
                                           IRandomGeneratorFactory randomGeneratorFactory,
                                           INavigationService navigationService,
                                           IAIDecisionHandler aIDecisionHandler) : base(game, engine, sceneLoader, animationManager, presenterStore, playerActionReceiverFactory, randomGeneratorFactory)
        {
            m_navigationService = navigationService;
            m_aIDecisionHandler = aIDecisionHandler;
        }

        protected override void InitializeGame()
        {
            base.InitializeGame();
            m_aIDecisionHandler.Initialize();
        }


        public override async Task OnGameOver()
        {
            await m_navigationService.NavigateToAsync("//MainPage");
        }

        private readonly INavigationService m_navigationService;
        private readonly IAIDecisionHandler m_aIDecisionHandler;
    }
}
