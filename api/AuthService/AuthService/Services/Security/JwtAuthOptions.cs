using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.Services.Security
{
    public class JwtAuthOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience {  get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public double AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

        public TimeSpan GetAccessTokenExpirationTimeSpan() =>
            TimeSpan.FromMinutes(AccessTokenExpirationMinutes);

        public TimeSpan GetRefreshTokenExpirationTimeSpan() =>
            TimeSpan.FromDays(RefreshTokenExpirationDays);
    }
}
