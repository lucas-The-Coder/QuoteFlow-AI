using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuoteFlowAI.DTOs;
using QuoteFlowAI.Helpers;
using QuoteFlowAI.Models;
using System.Threading.Tasks;

namespace QuoteFlowAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtHelper _jwtHelper;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, JwtHelper jwtHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Ensure the "User" role exists
                const string roleName = "User";
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                var user = new User
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedDate = System.DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                // Assign role
                await _userManager.AddToRoleAsync(user, roleName);

                return Ok(new { message = "User registered successfully" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";
            var token = _jwtHelper.GenerateToken(user.Id.ToString(), user.Email!, role, 60);

            return Ok(new AuthResponse
            {
                Token = token,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Expiration = System.DateTime.UtcNow.AddMinutes(60)
            });
        }
    }
}