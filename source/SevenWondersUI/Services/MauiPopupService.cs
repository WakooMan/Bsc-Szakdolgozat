using CommunityToolkit.Maui.Views;

namespace SevenWondersUI.Services
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
