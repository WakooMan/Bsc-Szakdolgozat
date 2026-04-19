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
using Microsoft.Extensions.DependencyInjection;
using SevenWonders.AI.Model;
using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;
using SevenWonders.AI.Model.DecisionRouter.Factories;
using SevenWonders.AI.Model.Services;
using SevenWonders.AI.Model.Services.CardTypeEncoders.Factories;
using SevenWonders.AI.Model.Services.Encoders;
using SevenWonders.AITrainerServer.DecisionHandlers;
using SevenWonders.AITrainerServer.PlayerActionReceivers;
using SevenWonders.Common;

namespace SevenWonders.AITrainerServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameLog.InitializeConsoleLogger();
            GameLog.Info("Starting AITrainerServer...");
            GameLog.Info($"Arguments: {string.Join(", ", args)}");
            var services = new ServiceCollection();

            services.AddSingleton<IXmlHandler, XmlHandler>();
            services.AddSingleton<IRandomGeneratorFactory, RandomGeneratorFactory>();
            services.AddKeyedSingleton<ICardListFactory, EmptyCardListFactory>(nameof(EmptyCardListFactory));
            services.AddKeyedSingleton<ICardListFactory, MainCardListFactory>(nameof(MainCardListFactory));
            services.AddSingleton<IWonderListFactory, WonderListFactory>();
            services.AddSingleton<IDevelopmentListFactory, DevelopmentListFactory>();
            services.AddSingleton<ICardCompositionFileHandlerFactory, CardCompositionFileHandlerFactory>();
            services.AddSingleton<ICardCompositionFactory, CardCompositionFactory>();
            services.AddSingleton<ICardNodeFactory, CardNodeFactory>();
            services.AddSingleton<IMilitaryBoardFactory, MilitaryBoardFactory>();
            services.AddSingleton<IPlayerActionHandler, PlayerActionHandler>();

            services.AddSingleton<ITurnHandler, TurnHandler>();
            services.AddSingleton<IAgeHandler, AgeHandler>();
            services.AddSingleton<ICostCalculator, CostCalculator>();
            services.AddSingleton<IChooseWonderHandler, ChooseWonderHandler>();
            services.AddSingleton<IGameElements, GameElements>();
            services.AddSingleton<IEventManager, EventManager>();
            services.AddSingleton<IGameContext, GameContext>();
            services.AddSingleton<IGame, Game>();

            services.AddSingleton<IEffectEncoder, EffectEncoder>();
            services.AddSingleton<ICardTypeEncoderFactory, CardTypeEncoderFactory>();
            services.AddSingleton<ICardNodeEncoder, CardNodeEncoder>();
            services.AddSingleton<ICardCompositionEncoder, CardCompositionEncoder>();
            services.AddSingleton<IPlayerEncoder, PlayerEncoder>();
            services.AddSingleton<IGlobalInfoEncoder, GlobalInfoEncoder>();
            services.AddSingleton<IGameStateVectorReceiver, GameStateVectorReceiver>();
            services.AddSingleton<IPlayerActionMaskReceiver, PlayerActionMaskReceiver>();

            services.AddSingleton<IWeightConfiguration, WeightConfiguration>();
            services.AddSingleton<IDecisionRouterFactory, DecisionRouterFactory>();
            services.AddSingleton<IAIDecisionHandler, AIDecisionHandler>();
            services.AddSingleton<IMilitaryHeuristicBotDecisionHandler, MilitaryHeuristicBotDecisionHandler>();
            services.AddSingleton<IScientificHeuristicBotDecisionHandler, ScientificHeuristicBotDecisionHandler>();
            services.AddSingleton<ICitizenHeuristicBotDecisionHandler, CitizenHeuristicBotDecisionHandler>();
            services.AddSingleton<IRandomBotDecisionHandler, RandomBotDecisionHandler>();
            services.AddSingleton<INonPlayerActionReceiverFactory, NonPlayerActionReceiverFactory>();
            services.AddSingleton<IAITrainerServer, AITrainerServer>();
            services.AddSingleton<IRewardCalculator, RewardCalculator>();

            GameLog.Info("Building service provider...");
            var serviceProvider = services.BuildServiceProvider();

            GameLog.Info("Resolving IAITrainerServer...");
            var server = serviceProvider.GetRequiredService<IAITrainerServer>();
            GameLog.Info("Starting server...");
            server.StartServer();
            GameLog.Info("Server stopped.");
        }
    }
}
