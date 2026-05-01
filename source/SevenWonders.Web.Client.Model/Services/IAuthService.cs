using SevenWonders.Web.Server.Contract;

namespace SevenWonders.Web.Client.Model.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(string username, string password);
        Task<RegisterResponse?> RegisterAsync(string username, string email, string password);
        Task<LogoutResponse?> LogoutAsync();
    }
}
