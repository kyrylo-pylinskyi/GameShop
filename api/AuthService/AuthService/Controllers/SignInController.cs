using AuthService.Models.DTO.Request;
using AuthService.Models.Entities;
using AuthService.Services.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth/[controller]")]
    public class SignInController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtAuthOptions _jwtAuthOptions;

        public SignInController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtAuthOptions jwtAuthOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpPost]
        [Route(nameof(SignIn))]
        public async Task<IActionResult> SignIn([FromForm] SignInRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.UserNameOrEmail) ?? 
                await _userManager.FindByNameAsync(request.UserNameOrEmail);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Unauthorized();
            }

            return Ok(JwtService.GetUserAuthTokens(_jwtAuthOptions, user));
        }

        [HttpPost]
        [Route(nameof(RefreshToken))]
        public async Task<IActionResult> RefreshToken(string requestToken)
        {
            var tokenClaims = JwtService.GetJwtClaims(requestToken);

            var user = await _userManager.FindByIdAsync(tokenClaims.NameId);

            if (user == null) return BadRequest(new { Message = "Invalid Token, user not found" });
            if (DateTime.UtcNow > tokenClaims.ExpirationDateTime) return BadRequest(new { Message = "Invalid Token, token expired" });

            return Ok(JwtService.GetUserAuthTokens(_jwtAuthOptions, user));
        }
    }
}
