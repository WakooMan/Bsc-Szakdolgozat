using GameLogic;
using Microsoft.Extensions.DependencyInjection;

namespace WebServer.Model
{
    public class GameFactory : IGameFactory
    {
        public GameFactory(IServiceProvider serviceProvider)
        {
            m_serviceProvider = serviceProvider;
        }

        public IGame Create()
        {
            var scope = m_serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IGame>();
        }

        private readonly IServiceProvider m_serviceProvider;
    }
}
