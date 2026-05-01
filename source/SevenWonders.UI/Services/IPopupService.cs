using CommunityToolkit.Maui.Views;

namespace SevenWonders.UI.Services
{
    public interface IPopupService
    {
        Task ShowAsync(Popup popup);
    }
}
