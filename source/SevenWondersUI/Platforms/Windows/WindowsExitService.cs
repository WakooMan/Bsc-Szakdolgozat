using SevenWondersUI.Services;

namespace SevenWondersUI.Platforms.Windows
{
    public class WindowsExitService : IExitService
    {
        public void ExitApplication()
        {
            var window = Application.Current?
            .Windows
            .FirstOrDefault();

            if (window != null)
            {
                Application.Current?.CloseWindow(window);
            }
        }
    }
}
