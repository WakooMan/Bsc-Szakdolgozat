using System.Net;
using System.Net.Http.Json;
using WebServer.Contract;

namespace SevenWondersUI.Services
{
    public class AuthService: IAuthService
    {
        public AuthService()
        {
            m_httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7206") };
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

        private readonly HttpClient m_httpClient;
    }
}
