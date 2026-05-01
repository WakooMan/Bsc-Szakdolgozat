using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Developments;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Military;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.GameStructures.Factories;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.Handlers.Factories;
using Microsoft.Extensions.DependencyInjection;
using SevenWonders.AI.Model;
using SevenWonders.AI.Model.AIModelHandler;
using SevenWonders.AI.Model.Cache;
using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;
using SevenWonders.AI.Model.DecisionRouter.Factories;
using SevenWonders.AI.Model.Factories;
using SevenWonders.AI.Model.Services.CardTypeEncoders.Factories;
using SevenWonders.AI.Model.Services.Encoders;
using SevenWonders.AI.Trainer.Server.DataModel;
using SevenWonders.AI.Trainer.Server.DecisionHandlers;
using SevenWonders.AI.Trainer.Server.PlayerActionReceivers;
using SevenWonders.Common;

namespace SevenWonders.AI.Trainer.Server
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
            services.AddSingleton<IGame, Game.Logic.Game>();

            services.AddSingleton<IEffectEncoder, EffectEncoder>();
            services.AddSingleton<ICardTypeEncoderFactory, CardTypeEncoderFactory>();
            services.AddSingleton<ICardCompositionEncoderFactory, CardCompositionEncoderFactory>();
            services.AddSingleton<IEasyCardNodeEncoder, EasyCardNodeEncoder>();
            services.AddSingleton<IEasyPlayerEncoder, EasyPlayerEncoder>();
            services.AddSingleton<IEasyGlobalInfoEncoder, EasyGlobalInfoEncoder>();
            services.AddSingleton<IMediumCardNodeEncoder, MediumCardNodeEncoder>();
            services.AddSingleton<IMediumPlayerEncoder, MediumPlayerEncoder>();
            services.AddSingleton<IMediumGlobalInfoEncoder, MediumGlobalInfoEncoder>();
            services.AddSingleton<IHardEncoderHelper, HardEncoderHelper>();
            services.AddSingleton<ICardEffectAnalyzer, CardEffectAnalyzer>();
            services.AddSingleton<IHardCardNodeEncoder, HardCardNodeEncoder>();
            services.AddSingleton<IHardPlayerEncoder, HardPlayerEncoder>();
            services.AddSingleton<IHardGlobalInfoEncoder, HardGlobalInfoEncoder>();
            services.AddSingleton<IGameStateVectorReceiverFactory, GameStateVectorReceiverFactory>();
            services.AddSingleton<IPlayerActionMaskReceiverFactory, PlayerActionMaskReceiverFactory>();

            services.AddSingleton<IWeightConfiguration, WeightConfiguration>();
            services.AddSingleton<IDecisionRouterFactory, DecisionRouterFactory>();
            services.AddSingleton<IAIDecisionHandlerCache, AIDecisionHandlerCache>();
            services.AddSingleton<IMilitaryHeuristicBotDecisionHandler, MilitaryHeuristicBotDecisionHandler>();
            services.AddSingleton<IScientificHeuristicBotDecisionHandler, ScientificHeuristicBotDecisionHandler>();
            services.AddSingleton<ICitizenHeuristicBotDecisionHandler, CitizenHeuristicBotDecisionHandler>();
            services.AddSingleton<IRandomBotDecisionHandler, RandomBotDecisionHandler>();
            services.AddSingleton<INonPlayerActionReceiverFactory, NonPlayerActionReceiverFactory>();
            services.AddSingleton<IAITrainerServer, AITrainerServer>();
            services.AddSingleton<IRewardCalculatorFactory, RewardCalculatorFactory>();
            services.AddSingleton<IPathProvider, PathProvider>();
            services.AddSingleton<IAIModelHandlerCache, AIModelHandlerCache>();
            services.AddSingleton<IEnemyChanceConfiguration, EnemyChanceConfiguration>();

            GameLog.Info("Building service provider...");
            var serviceProvider = services.BuildServiceProvider();

            GameLog.Info("Resolving IAITrainerServer...");
            var server = serviceProvider.GetRequiredService<IAITrainerServer>();
            GameLog.Info("Starting server...");
            server.StartServer().GetAwaiter().GetResult();
            GameLog.Info("Server stopped.");
        }
    }
}
