using System.Collections.ObjectModel;
using WebServer.Contract.DataTransferObjects;

namespace SevenWondersUI.ViewModels
{
    public class LeaderboardPopupViewModel : BaseViewModel
    {
        public string Title => "Ranglista";
        public string CloseButtonText => "Bezárás";

        public ObservableCollection<LeaderboardEntryDto> Entries { get; }

        public LeaderboardPopupViewModel(LeaderboardEntryDto[] entries)
        {
            Entries = new ObservableCollection<LeaderboardEntryDto>(entries);
        }
    }
}
