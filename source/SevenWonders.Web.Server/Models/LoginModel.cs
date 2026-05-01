namespace SevenWonders.Web.Server.Models
{
    public class LoginModel
    {
        public LoginModel()
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
