using SevenWondersUI.Services;

namespace SevenWondersUI.Platforms.Android
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
