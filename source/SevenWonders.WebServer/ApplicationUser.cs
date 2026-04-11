using Microsoft.AspNetCore.Identity;

namespace SevenWonders.WebServer
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public bool IsOnline { get; set; } = false;
    }
}
