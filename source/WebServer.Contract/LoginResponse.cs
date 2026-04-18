namespace WebServer.Contract
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }

        public LoginResponse() { Message = string.Empty; Token = string.Empty; }

        public LoginResponse(bool success, string message, string token) { Success = success; Message = message; Token = token; }
    }
}
