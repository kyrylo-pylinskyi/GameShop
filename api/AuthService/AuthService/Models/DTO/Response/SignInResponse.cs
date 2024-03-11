namespace AuthService.Models.DTO.Response
{
    public class SignInResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
