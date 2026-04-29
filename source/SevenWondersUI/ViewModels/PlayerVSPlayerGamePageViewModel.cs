using SevenWonders.Common;
using SevenWonders.Presenter;
using SevenWondersUI.Services;

namespace SevenWondersUI.ViewModels
{
    public class PlayerVSPlayerGamePageViewModel: BaseGamePageViewModel
    {
        protected override RandomGeneratorType RandomGeneratorType => RandomGeneratorType.Undeterministic;

        protected override PlayerType Player1Type => PlayerType.LocalPlayer;

        protected override PlayerType Player2Type => PlayerType.LocalPlayer;

        public PlayerVSPlayerGamePageViewModel(IGameHandler gameHandler, INavigationService navigationService) : base(gameHandler, navigationService)
        {
        }
    }
}
