using GameLogic;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.PlayerActionReceivers;
using SevenWonders.Presenter.Presenters;
using SevenWondersUI.Services;

namespace SevenWondersUI.ViewModels
{
    public class PlayerVSPlayerGamePageViewModel: BaseGamePageViewModel
    {
        protected override RandomGeneratorType RandomGeneratorType => RandomGeneratorType.Undeterministic;

        protected override PlayerType Player1Type => PlayerType.LocalPlayer;

        protected override PlayerType Player2Type => PlayerType.LocalPlayer;

        public PlayerVSPlayerGamePageViewModel(IGame game,
                                               IEngine engine,
                                               ISceneLoader sceneLoader,
                                               IAnimationManager animationManager,
                                               IPresenterStore presenterStore,
                                               IPlayerActionReceiverFactory playerActionReceiverFactory,
                                               IRandomGeneratorFactory randomGeneratorFactory,
                                               INavigationService navigationService) : base(game, engine, sceneLoader, animationManager, presenterStore, playerActionReceiverFactory, randomGeneratorFactory)
        {
            m_navigationService = navigationService;
        }
        

        public override async Task OnGameOver()
        {
            await m_navigationService.NavigateToAsync("//MainPage");
        }

        private readonly INavigationService m_navigationService;
    }
}
