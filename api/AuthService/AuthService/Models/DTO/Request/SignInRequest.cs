using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTO.Request
{
    public class SignInRequest
    {
        [Required(ErrorMessage = "Field can't be empty")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Must be between 5 and 30 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
