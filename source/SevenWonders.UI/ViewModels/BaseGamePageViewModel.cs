using SevenWonders.Common;
using SevenWonders.Game.Presenter;
using SevenWonders.UI.Services;

namespace SevenWonders.UI.ViewModels
{
    public abstract class BaseGamePageViewModel: BaseViewModel, IGameOverHandler, IQueryAttributable
    {
        protected BaseGamePageViewModel(IGameHandler gameHandler, INavigationService navigationService)
        {
            m_gameHandler = gameHandler;
            m_appliedQueryAttributes = false;
            Player1Name = string.Empty;
            Player2Name = string.Empty;
            m_navigationService=navigationService;
        }

        public IGameHandler GameHandler => m_gameHandler;

        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        protected abstract RandomGeneratorType RandomGeneratorType { get; }
        protected abstract PlayerType Player1Type { get; }
        protected abstract PlayerType Player2Type { get; }

        protected virtual int Seed { get { return 0; } }

        protected virtual int StartingPlayerId { get { return 1; } }

        public async Task Initialize()
        {
            while(!m_appliedQueryAttributes)
            {
                await Task.Delay(100);
            }

            await m_gameHandler.StartGame(Player1Name, 
                                    Player1Type, 
                                    Player2Name, 
                                    Player2Type, 
                                    RandomGeneratorType, 
                                    Seed, 
                                    StartingPlayerId, 
                                    this);
        }

        public virtual async Task OnGameOver()
        {
            GameHandler.StopGame();
            await m_navigationService.NavigateToAsync("//MainPage");
        }
        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Player1Name", out object? player1NameObj) && player1NameObj is string player1Name)
            {
                Player1Name = player1Name;
            }
            if (query.TryGetValue("Player2Name", out object? player2NameObj) && player2NameObj is string player2Name)
            {
                Player2Name = player2Name;
            }
            m_appliedQueryAttributes = true;
        }

        private readonly IGameHandler m_gameHandler;
        private readonly INavigationService m_navigationService;
        private bool m_appliedQueryAttributes;
    }
}
