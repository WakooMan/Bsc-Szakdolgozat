using System.ComponentModel.DataAnnotations;

namespace SevenWonders.Web.Server.Models
{
    public class RegisterModel
    {
        public RegisterModel()
        {
            Email = string.Empty;
            Password = string.Empty;
            Username = string.Empty;
        }

        [Required]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail cím!")]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "A jelszó legalább 6 karakter legyen.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A felhasználónév kötelező!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "A név 3 és 20 karakter között legyen.")]
        public string Username { get; set; }
    }
}
