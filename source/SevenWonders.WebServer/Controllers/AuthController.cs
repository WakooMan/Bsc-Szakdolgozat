using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SevenWonders.WebServer.Models;
using WebServer.Model.Client;

namespace SevenWonders.WebServer.Controllers
{
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
                return Ok(new { Message = "Successful registration!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await m_signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Successful login!" });
            }

            return BadRequest("Username or Password is wrong.");
        }

        private readonly UserManager<ApplicationUser> m_userManager;
        private readonly SignInManager<ApplicationUser> m_signInManager;
    }
}
