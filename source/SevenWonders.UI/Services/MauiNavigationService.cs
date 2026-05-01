namespace SevenWonders.UI.Services
{
    public class MauiNavigationService : INavigationService
    {
        public async Task InitializeAsync()
        {
            if (Shell.Current == null)
            {
                await Task.Delay(100);
            }

            if (Shell.Current != null)
            {
                await NavigateToAsync("//MainPage");
            }
        }

        public async Task NavigateToAsync(string route, IDictionary<string, object>? routeParameters = null)
        {
            await (routeParameters != null
            ? Shell.Current.GoToAsync(route, routeParameters)
            : Shell.Current.GoToAsync(route));
        }
    }
}
