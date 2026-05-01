using SevenWonders.Web.Server.Model.PlayerStates.Factories;

namespace SevenWonders.Web.Server.Model.Client.Factories
{
    public class PlayerClientFactory : IPlayerClientFactory
    {
        public PlayerClientFactory(IPlayerStateFactory playerStateFactory)
        {
            m_playerStateFactory = playerStateFactory;
        }

        public IPlayerClient Create(ApplicationUser applicationUser, string connectionId)
        {
            return new PlayerClient(m_playerStateFactory, applicationUser, connectionId);
        }

        private readonly IPlayerStateFactory m_playerStateFactory;
    }
}
