using Microsoft.AspNetCore.Identity;

namespace WebServer.Model.Client
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public bool IsOnline { get; set; } = false;
    }
}
