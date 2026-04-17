using SevenWonders.Presenter;

namespace SevenWondersUI.Services
{
    public class SingleplayerGameOverHandler: IGameOverHandler
    {
        public SingleplayerGameOverHandler(INavigationService navigationService) 
        {
            m_navigationService = navigationService;
        }

        public async Task OnGameOver()
        {
            await m_navigationService.NavigateToAsync("//MainPage");
        }

        private readonly INavigationService m_navigationService;
    }
}
