using CommunityToolkit.Maui.Views;

namespace SevenWonders.UI.Services
{
    public class MauiPopupService : IPopupService
    {
        public async Task ShowAsync(Popup popup)
        {
            var page = Shell.Current?.CurrentPage;
            if (page != null)
            {
                await page.ShowPopupAsync(popup);
            }
        }
    }
}
