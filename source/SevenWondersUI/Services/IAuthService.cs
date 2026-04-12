using WebServer.Contract;

namespace SevenWondersUI.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(string username, string password);
        Task<RegisterResponse?> RegisterAsync(string username, string email, string password);
    }
}
