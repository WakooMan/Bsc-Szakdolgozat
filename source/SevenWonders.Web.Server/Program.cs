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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SevenWonders.Common;
using System.Text;
using System.Text.Json;
using SevenWonders.Web.Server.Model;
using SevenWonders.Web.Server.Model.Client;
using SevenWonders.Web.Server.Model.Client.Factories;
using SevenWonders.Web.Server.Model.Lobby;
using SevenWonders.Web.Server.Model.Matchmaking;
using SevenWonders.Web.Server.Model.MessageHandling;
using SevenWonders.Web.Server.Model.MessageHandling.Factories;
using SevenWonders.Web.Server.Model.PlayerStates.Factories;
using SevenWonders.Web.Server.Model.ServerHub;

namespace SevenWonders.Web.Server
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

            var signingKey = builder.Configuration["Jwt:SigningKey"]
                ?? throw new InvalidOperationException("JWT signing key missing");

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey)),
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
            builder.Services.AddSingleton(typeof(IRandomGeneratorFactory), typeof(RandomGeneratorFactory));
            builder.Services.AddKeyedSingleton<ICardListFactory, EmptyCardListFactory>(nameof(EmptyCardListFactory));
            builder.Services.AddKeyedSingleton<ICardListFactory, MainCardListFactory>(nameof(MainCardListFactory));
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
            builder.Services.AddTransient(typeof(IGame), typeof(Game.Logic.Game));
            builder.Services.AddSingleton(typeof(IGameFactory), typeof(GameFactory));

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
            builder.Services.AddSingleton<IMatchmakingService, MatchmakingService>();

            var app = builder.Build();

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
