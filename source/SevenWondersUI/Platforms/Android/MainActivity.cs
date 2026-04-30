using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace SevenWondersUI;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);



        if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
        {
            if (Window is not null)
            {
#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1422 // Validate platform compatibility
                Window.SetDecorFitsSystemWindows(false);


                var controller = Window.InsetsController;
                if (controller != null)
                {
                    controller.Hide(WindowInsets.Type.StatusBars() | WindowInsets.Type.NavigationBars());
                    controller.SystemBarsBehavior =
                        (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;
#pragma warning restore CA1422 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
                }
            }
        }
        else
        {
#pragma warning disable CS0618
            if (Window is not null && Window.DecorView is not null)
            {
                Window.DecorView.SystemUiVisibility =
                    (StatusBarVisibility)(
                        SystemUiFlags.Fullscreen |
                        SystemUiFlags.HideNavigation |
                        SystemUiFlags.ImmersiveSticky);
            }
#pragma warning restore CS0618
        }


    }
}
