namespace AuthService.Services.Security.DTO
{
    public class JwtClaims
    {
        public string NameId { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }
}
