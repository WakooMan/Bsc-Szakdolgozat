using System.Net.Http.Json;
using SevenWonders.Web.Server.Contract;

namespace SevenWonders.Web.Client.Model.Services
{
    public class AuthService: IAuthService
    {
        public AuthService(INetworkConfiguration networkConfiguration)
        {
            m_networkConfiguration = networkConfiguration;
            m_httpClient = new HttpClient
            {
                BaseAddress = m_networkConfiguration.ApiBaseUri, 
                Timeout = m_networkConfiguration.HttpTimeout 
            };
        }

        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            var response = await m_httpClient.PostAsJsonAsync("api/auth/login", new { username, password });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }
            return new LoginResponse(false, response?.ReasonPhrase ?? string.Empty, string.Empty);
        }

        public async Task<RegisterResponse?> RegisterAsync(string username, string email, string password)
        {
            var response = await m_httpClient.PostAsJsonAsync("api/auth/register", new { username, email, password });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RegisterResponse>();
            }
            return new RegisterResponse(false, response?.ReasonPhrase ?? string.Empty);
        }

        public async Task<LogoutResponse?> LogoutAsync()
        {
            var response = await m_httpClient.PostAsync("api/auth/logout", null);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LogoutResponse>();
            }
            return new LogoutResponse(false, response?.ReasonPhrase ?? string.Empty);
        }

        private readonly HttpClient m_httpClient;
        private readonly INetworkConfiguration m_networkConfiguration;
    }
}
