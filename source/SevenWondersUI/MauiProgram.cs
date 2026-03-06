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
using GameLogic.PlayerActions;
using Microsoft.Extensions.Logging;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Animations;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.PlayerActionHandler;
using SevenWonders.Presenter.PlayerActionReceivers;
using SevenWonders.Presenter.PlayerActionWaiters;
using SevenWonders.Presenter.Presenters;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;
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
			.UseSkiaSharp()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton(typeof(IXmlHandler), typeof(XmlHandler));
        builder.Services.AddSingleton(typeof(IRandomGenerator), typeof(RandomGenerator));
        builder.Services.AddKeyedSingleton<ICardListFactory, EmptyCardListFactory>(nameof(EmptyCardListFactory));
        builder.Services.AddKeyedSingleton<ICardListFactory, MainCardListFactory>(nameof(MainCardListFactory));
        builder.Services.AddSingleton(typeof(IRandomElementReceiver), typeof(RandomElementReceiver));
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
        builder.Services.AddSingleton(typeof(IPlayerActionReceiver), typeof(PlayerActionReceiver));
        builder.Services.AddSingleton(typeof(IPlayerActionWaiter<ChooseWonderAction>), typeof(ChooseWonderActionWaiter));
        builder.Services.AddSingleton(typeof(IPlayerActionWaiter<PickCard>), typeof(PickCardActionWaiter));
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
        builder.Services.AddSingleton(typeof(IGameObjectReceiver), typeof(GameObjectReceiver));

        builder.Services.AddSingleton(typeof(IWonderConnector), typeof(WonderConnector));
        builder.Services.AddSingleton(typeof(IWonderPresenter), typeof(WonderPresenter));
        builder.Services.AddSingleton(typeof(ICardConnector), typeof(CardConnector));
        builder.Services.AddSingleton(typeof(ICardPresenter), typeof(CardPresenter));

        builder.Services.AddTransient<MainPage>();


        return builder.Build();
	}
}
