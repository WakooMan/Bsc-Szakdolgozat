using Microsoft.AspNetCore.Identity;

namespace SevenWonders.Web.Server.Model.Client
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public bool IsOnline { get; set; } = false;
        public int CompetitiveWins { get; set; } = 0;
    }
}
