using AuthService.Models.DTO.Request;
using AuthService.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth/[controller]")]
    public class SignUpController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public SignUpController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] SignUpRequest request)
        {
            var user = _mapper.Map<ApplicationUser>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action("ConfirmEmail", "SignUp", new { userId = user.Id, token }, Request.Scheme);

            // Send Email throw microservice

            return Ok(new { Message = "OK", user.Id, token, callbackUrl });
        }

        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token)) return BadRequest("User Id and token can not be null");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new {Message = "OK", result});
        }

        [HttpPost]
        [Route(nameof(ResendVerificationLink))]
        public async Task<IActionResult> ResendVerificationLink(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest("User not found");
            
            if (user.EmailConfirmed) return BadRequest("Email already confirmed");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action("ConfirmEmail", "SignUp", new { userId = user.Id, token }, Request.Scheme);

            // Send Email throw microservice

            return Ok(new { Message = "OK", user.Id, token, callbackUrl });
        }
    }
}
