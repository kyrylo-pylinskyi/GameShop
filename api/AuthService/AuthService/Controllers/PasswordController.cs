using Api.Models.DTO.Requests.AuthRequests.SignIn;
using AuthService.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(ForgotPassword))]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Password", new { userId = user.Id, token }, Request.Scheme);

            // TODO Send Email throw Email microservice

            return Ok(new { Message = "OK", user.Email, callbackUrl });
        }

        [HttpPost]
        [Route(nameof(ResetPassword))]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            // TODO Send Email throw Email microservice

            return Ok(new { Message = "OK", result, user.Email, request.Password });
        }
    }
}
