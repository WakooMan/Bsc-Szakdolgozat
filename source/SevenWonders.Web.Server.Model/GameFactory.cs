using SevenWonders.Game.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace SevenWonders.Web.Server.Model
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
