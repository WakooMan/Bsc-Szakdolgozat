using System.Collections.ObjectModel;
using SevenWonders.Web.Server.Contract.DataTransferObjects;

namespace SevenWonders.UI.ViewModels
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
