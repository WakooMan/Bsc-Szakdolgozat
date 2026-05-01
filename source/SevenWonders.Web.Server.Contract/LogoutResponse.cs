namespace SevenWonders.Web.Server.Contract
{
    public class LogoutResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public LogoutResponse() { Message = string.Empty; }

        public LogoutResponse(bool success, string message) { Success = success; Message = message; }
    }
}
