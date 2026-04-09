using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.SceneEditor.Helpers;
using SevenWonders.SceneEditor.ViewModels;
using SevenWonders.SceneEditor.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace SevenWonders.SceneEditor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Cinzel-Bold.ttf", "CinzelBold");
                    fonts.AddFont("Cinzel-Regular.ttf", "CinzelRegular");
                }).
                RegisterGameEngine().
                RegisterViewModels().
                RegisterViews();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPageViewModel>();

            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<MainPage>();

            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterGameEngine(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton(typeof(IXmlHandler), typeof(XmlHandler));
            mauiAppBuilder.Services.AddSingleton(typeof(IRandomGenerator), typeof(RandomGenerator));
            mauiAppBuilder.Services.AddSingleton(typeof(IAnimationManager), typeof(AnimationManager));
            mauiAppBuilder.Services.AddSingleton(typeof(IGameEngineTicker), typeof(GameEngineTicker));
            mauiAppBuilder.Services.AddSingleton(typeof(ISceneLoader), typeof(SceneLoader));
            mauiAppBuilder.Services.AddSingleton(typeof(IObjectManager), typeof(ObjectManager));
            mauiAppBuilder.Services.AddSingleton(typeof(ISceneManager), typeof(SceneManager));
            mauiAppBuilder.Services.AddSingleton(typeof(IInputManager), typeof(InputManager));
            mauiAppBuilder.Services.AddSingleton(typeof(IZipFileReceiver), typeof(NormalZipFileReceiver));
            mauiAppBuilder.Services.AddSingleton(typeof(IEngine), typeof(Engine));

            return mauiAppBuilder;
        }
    }
}
