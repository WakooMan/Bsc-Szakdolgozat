using SevenWonders.UI.Services;

namespace SevenWonders.UI.Platforms.Android
{
    public class AndroidExitService: IExitService
    {
        public void ExitApplication()
        {
            var activity = Platform.CurrentActivity;
            activity?.FinishAffinity();
        }
    }
}
