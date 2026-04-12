using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SevenWonders.WebServer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebServer.Contract;
using WebServer.Model.Client;

namespace SevenWonders.WebServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            m_userManager = userManager;
            m_signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await m_userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new RegisterResponse(true, "Successful registration!"));
            }

            return BadRequest(new RegisterResponse(false, string.Join('\n', result.Errors.Select(error => error.ToString()).ToArray())));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await m_userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return Unauthorized(new LoginResponse(false, "Érvénytelen felhasználónév vagy jelszó.", string.Empty));
            }

            var result = await m_signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                return Ok(new LoginResponse(true, "Sikeres bejelentkezés", token));
            }

            return Unauthorized(new LoginResponse(false, "Hibás jelszó.", string.Empty));
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Nagyon_Titkos_Es_Hosszu_Kulcs_123456789");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private readonly UserManager<ApplicationUser> m_userManager;
        private readonly SignInManager<ApplicationUser> m_signInManager;
    }
}
