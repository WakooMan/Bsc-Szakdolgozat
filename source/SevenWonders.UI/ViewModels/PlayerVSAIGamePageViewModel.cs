using SevenWonders.Common;
using SevenWonders.Game.Presenter;
using SevenWonders.UI.Services;

namespace SevenWonders.UI.ViewModels
{
    public class PlayerVSAIGamePageViewModel : BaseGamePageViewModel
    {
        protected override RandomGeneratorType RandomGeneratorType => RandomGeneratorType.Undeterministic;

        protected override PlayerType Player1Type => PlayerType.LocalPlayer;

        protected override PlayerType Player2Type => m_player2Type;

        public PlayerVSAIGamePageViewModel(IGameHandler gameHandler,
                                           INavigationService navigationService) : base(gameHandler, navigationService)
        {
            m_player2Type = PlayerType.EasyAI;
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Player2Type", out object? obj) && obj is PlayerType player2Type)
            {
                m_player2Type = player2Type;
            }
            base.ApplyQueryAttributes(query);
        }

        private PlayerType m_player2Type;
    }
}
