using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SevenWonders.Common;
using System.Text;
using WebServer.Model.Client;
using WebServer.Model.Client.Factories;
using WebServer.Model.Lobby;
using WebServer.Model.MessageHandling;
using WebServer.Model.MessageHandling.Factories;
using WebServer.Model.PlayerStates.Factories;

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
            });

            builder.Services.AddSingleton<IRandomGenerator, RandomGenerator>();
            builder.Services.AddSingleton<IXmlHandler, XmlHandler>();
            builder.Services.AddSingleton<IClientManager, ClientManager>();
            builder.Services.AddSingleton<ILobbyFactory, LobbyFactory>();
            builder.Services.AddSingleton<ILobbyManager, LobbyManager>();
            builder.Services.AddSingleton<IPlayerStateFactory, PlayerStateFactory>();
            builder.Services.AddSingleton<IPlayerClientFactory, PlayerClientFactory>();
            builder.Services.AddSingleton<ILobbyCodeGenerator, LobbyCodeGenerator>();
            builder.Services.AddSingleton<IMessageRegistererFactory, MessageRegistererFactory>();
            builder.Services.AddSingleton<ILobbyMessageHandlers, LobbyMessageHandlers>();
            builder.Services.AddSingleton<IServerMessageDispatcher, ServerMessageDispatcher>();

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
