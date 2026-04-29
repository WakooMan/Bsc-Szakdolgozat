using CommunityToolkit.Maui.Views;

namespace SevenWondersUI.Services
{
    public interface IPopupService
    {
        Task ShowAsync(Popup popup);
    }
}
