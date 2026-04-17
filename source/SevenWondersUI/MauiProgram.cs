using CommunityToolkit.Maui;
using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Military;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;
using GameLogic.Handlers.Factories;
using GameLogic.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Connectors.Cards;
using SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers;
using SevenWonders.Presenter.Connectors.Developments;
using SevenWonders.Presenter.Connectors.Effects;
using SevenWonders.Presenter.Connectors.MilitaryBoard;
using SevenWonders.Presenter.Connectors.Wonders;
using SevenWonders.Presenter.Connectors.Wonders.WonderChildTextureHandlers;
using SevenWonders.Presenter.PlayerActionReceivers;
using SevenWonders.Presenter.Presenters;
using SevenWonders.Presenter.Presenters.Factories;
using SevenWonders.Presenter.Views.Factories;
using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Factories;
using SevenWonders.WebClient.Model.Services;
using SevenWondersUI.Services;
using SevenWondersUI.Services.Factories;
using SevenWondersUI.ViewModels;
using SevenWondersUI.Views;
using SevenWondersUI.Views.Factories;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace SevenWondersUI;

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
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton(typeof(IXmlHandler), typeof(XmlHandler));
        builder.Services.AddSingleton(typeof(IRandomGenerator), typeof(DefaultRandomGenerator));
        builder.Services.AddKeyedSingleton<ICardListFactory, EmptyCardListFactory>(nameof(EmptyCardListFactory));
        builder.Services.AddKeyedSingleton<ICardListFactory, MainCardListFactory>(nameof(MainCardListFactory));
        builder.Services.AddSingleton(typeof(IRandomGeneratorFactory), typeof(RandomGeneratorFactory));
        builder.Services.AddSingleton(typeof(IWonderListFactory), typeof(WonderListFactory));
        builder.Services.AddSingleton(typeof(IDevelopmentListFactory), typeof(DevelopmentListFactory));
        builder.Services.AddSingleton(typeof(IGameElements), typeof(GameElements));
        builder.Services.AddSingleton(typeof(IEventManager), typeof(EventManager));
        builder.Services.AddSingleton(typeof(ICardCompositionFileHandlerFactory), typeof(CardCompositionFileHandlerFactory));
        builder.Services.AddSingleton(typeof(ICostCalculator), typeof(CostCalculator));
        builder.Services.AddSingleton(typeof(IChooseWonderHandler), typeof(ChooseWonderHandler));
        builder.Services.AddSingleton(typeof(ICardCompositionFactory), typeof(CardCompositionFactory));
        builder.Services.AddSingleton(typeof(ICardNodeFactory), typeof(CardNodeFactory));
        builder.Services.AddSingleton(typeof(ITurnHandler), typeof(TurnHandler));
        builder.Services.AddSingleton(typeof(IAgeHandler), typeof(AgeHandler));
        builder.Services.AddSingleton(typeof(IMilitaryBoardFactory), typeof(MilitaryBoardFactory));
        builder.Services.AddSingleton(typeof(IPlayerActionReceiver), typeof(LocalPlayerActionReceiver));
        builder.Services.AddSingleton(typeof(IPlayerActionHandler), typeof(PlayerActionHandler));
        builder.Services.AddSingleton(typeof(IGameContext), typeof(GameContext));
        builder.Services.AddSingleton(typeof(IGame), typeof(Game));
        builder.Services.AddSingleton(typeof(IAnimationManager), typeof(AnimationManager));
        builder.Services.AddSingleton(typeof(IGameEngineTicker), typeof(GameEngineTicker));
        builder.Services.AddSingleton(typeof(ISceneLoader), typeof(SceneLoader));
        builder.Services.AddSingleton(typeof(IObjectManager), typeof(ObjectManager));
        builder.Services.AddSingleton(typeof(ISceneManager), typeof(SceneManager));
        builder.Services.AddSingleton(typeof(IInputManager), typeof(InputManager));
        builder.Services.AddSingleton(typeof(IZipFileReceiver), typeof(MauiZipFileReceiver));
        builder.Services.AddSingleton(typeof(IEngine), typeof(Engine));
        builder.Services.AddSingleton(typeof(IGameObjectViewFactory), typeof(GameObjectViewFactory));
        builder.Services.AddSingleton(typeof(IAnimationGroupBuilderFactory), typeof(AnimationGroupBuilderFactory));
        builder.Services.AddSingleton(typeof(IGameEngineReceiver), typeof(GameEngineReceiver));
        builder.Services.AddSingleton(typeof(ICardChildTextureHandler), typeof(CardChildTextureHandler));
        builder.Services.AddSingleton(typeof(IDevelopmentConnector), typeof(DevelopmentConnector));
        builder.Services.AddSingleton(typeof(ITextureIdHandler), typeof(TextureIdHandler));
        builder.Services.AddSingleton(typeof(IMilitaryTokenChildTextureHandler), typeof(MilitaryTokenChildTextureHandler));
        builder.Services.AddSingleton(typeof(IDevelopmentHandlerFactory), typeof(DevelopmentHandlerFactory));

        builder.Services.AddSingleton(typeof(IEffectHandler), typeof(EffectHandler));
        builder.Services.AddSingleton(typeof(IPlayerCardHandlerFactory), typeof(PlayerCardHandlerFactory));
        builder.Services.AddSingleton(typeof(IWonderChildTextureHandler), typeof(WonderChildTextureHandler));
        builder.Services.AddSingleton(typeof(IWonderConnector), typeof(WonderConnector));
        builder.Services.AddSingleton(typeof(IPresenterFactory), typeof(PresenterFactory));
        builder.Services.AddSingleton(typeof(IPresenter), typeof(Presenter));
        builder.Services.AddSingleton(typeof(ICardConnector), typeof(CardConnector));
        builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton(typeof(IGameOverHandlerFactory), typeof(GameOverHandlerFactory));
        builder.Services.AddSingleton(typeof(IClientHubService), typeof(ClientHubService));
        builder.Services.AddSingleton(typeof(IClientMessageDispatcher), typeof(ClientMessageDispatcher));
        builder.Services.AddSingleton(typeof(IMessageRegistererFactory), typeof(MessageRegistererFactory));
        builder.Services.AddSingleton(typeof(IPlayerActionReceiverFactory), typeof(PlayerActionReceiverFactory));
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<App>();

        builder.Services.AddTransient<LobbyMainPageViewModel>();
        builder.Services.AddTransient<LobbyMainPage>();

        builder.Services.AddTransient<ConnectPageViewModel>();
        builder.Services.AddTransient<ConnectPage>();

        builder.Services.AddTransient<LoginPageViewModel>();
        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<GamePageViewModel>();
        builder.Services.AddTransient<GamePage>();


        builder.Services.AddTransient<PlayerNamePageViewModel>();
        builder.Services.AddTransient<PlayerNamePage>();

		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<MainPage>();

		builder.Services.AddTransient<CreateGamePopupViewModel>();
		builder.Services.AddTransient<CreateGamePopupWindow>();

        builder.Services.AddTransient<LobbyPageViewModel>();
        builder.Services.AddTransient<LobbyPage>();


        return builder.Build();
	}
}
