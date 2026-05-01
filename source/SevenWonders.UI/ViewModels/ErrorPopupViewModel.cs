namespace SevenWonders.UI.ViewModels
{
    public class ErrorPopupViewModel : BaseViewModel
    {
        private string m_errorMessage = string.Empty;

        public string OkButtonText => "OK";

        public string ErrorMessage
        {
            get => m_errorMessage;
            set { m_errorMessage = value; OnPropertyChanged(); }
        }

        public ErrorPopupViewModel(string errorMessage)
        {
            m_errorMessage = errorMessage;
        }
    }
}
