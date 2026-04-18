namespace WebServer.Model.Client.Factories
{
    public interface IPlayerClientFactory
    {
        IPlayerClient Create(ApplicationUser applicationUser, string connectionId);
    }
}
