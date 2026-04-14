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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SevenWonders.Common;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using WebServer.Model;
using WebServer.Model.Client;
using WebServer.Model.Client.Factories;
using WebServer.Model.Lobby;
using WebServer.Model.MessageHandling;
using WebServer.Model.MessageHandling.Factories;
using WebServer.Model.PlayerStates.Factories;
using WebServer.Model.ServerHub;

namespace SevenWonders.WebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Nagyon_Titkos_Es_Hosszu_Kulcs_123456789")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/serverhub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.PayloadSerializerOptions.TypeInfoResolverChain.Insert(
                    0,
                    JsonSerializerOptions.Default.TypeInfoResolver!
                );
            });

            builder.Services.AddSingleton(typeof(IXmlHandler), typeof(XmlHandler));
            builder.Services.AddSingleton(typeof(IRandomGenerator), typeof(RandomGenerator));
            builder.Services.AddKeyedSingleton<ICardListFactory, EmptyCardListFactory>(nameof(EmptyCardListFactory));
            builder.Services.AddKeyedSingleton<ICardListFactory, MainCardListFactory>(nameof(MainCardListFactory));
            builder.Services.AddSingleton(typeof(IRandomElementReceiver), typeof(RandomElementReceiver));
            builder.Services.AddSingleton(typeof(IWonderListFactory), typeof(WonderListFactory));
            builder.Services.AddSingleton(typeof(IDevelopmentListFactory), typeof(DevelopmentListFactory));
            builder.Services.AddSingleton(typeof(ICardCompositionFileHandlerFactory), typeof(CardCompositionFileHandlerFactory));
            builder.Services.AddSingleton(typeof(ICardCompositionFactory), typeof(CardCompositionFactory));
            builder.Services.AddSingleton(typeof(ICardNodeFactory), typeof(CardNodeFactory));
            builder.Services.AddSingleton(typeof(IMilitaryBoardFactory), typeof(MilitaryBoardFactory));
            builder.Services.AddSingleton(typeof(IPlayerActionHandler), typeof(PlayerActionHandler));

            builder.Services.AddTransient(typeof(ITurnHandler), typeof(TurnHandler));
            builder.Services.AddTransient(typeof(IAgeHandler), typeof(AgeHandler));
            builder.Services.AddTransient(typeof(ICostCalculator), typeof(CostCalculator));
            builder.Services.AddTransient(typeof(IChooseWonderHandler), typeof(ChooseWonderHandler));
            builder.Services.AddTransient(typeof(IGameElements), typeof(GameElements));
            builder.Services.AddTransient(typeof(IEventManager), typeof(EventManager));
            builder.Services.AddTransient(typeof(IGameContext), typeof(GameContext));
            builder.Services.AddTransient(typeof(IGame), typeof(Game));
            builder.Services.AddSingleton(typeof(IGameInitializer), typeof(GameInitializer));

            builder.Services.AddSingleton<IClientManager, ClientManager>();
            builder.Services.AddSingleton<ILobbyFactory, LobbyFactory>();
            builder.Services.AddSingleton<ILobbyManager, LobbyManager>();
            builder.Services.AddSingleton<IPlayerStateFactory, PlayerStateFactory>();
            builder.Services.AddSingleton<IPlayerClientFactory, PlayerClientFactory>();
            builder.Services.AddSingleton<ILobbyCodeGenerator, LobbyCodeGenerator>();
            builder.Services.AddSingleton<IMessageRegistererFactory, MessageRegistererFactory>();
            builder.Services.AddSingleton<ILobbyMessageHandlers, LobbyMessageHandlers>();
            builder.Services.AddSingleton<IServerMessageDispatcher, ServerMessageDispatcher>();
            builder.Services.AddSingleton<IServerService, ServerService>();
            builder.Services.AddSingleton(typeof(IGameManager), typeof(GameManager));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<ServerHub>("/serverhub");
            app.Run();
        }
    }
}
