using AuthService.Models.DTO.Response;
using AuthService.Models.Entities;
using AuthService.Services.Security.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services.Security
{
    public class JwtService
    {
        public static SignInResponse GetUserAuthTokens(JwtAuthOptions authOptions, ApplicationUser user)
        {
            var accessTokenClaims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            });

            var refreshTokenClaims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            });

            return new SignInResponse
            {
                AccessToken = GetJwt(authOptions, accessTokenClaims, authOptions.GetAccessTokenExpirationTimeSpan()),
                RefreshToken = GetJwt(authOptions, refreshTokenClaims, authOptions.GetRefreshTokenExpirationTimeSpan())
            };
        }

        public static JwtClaims GetJwtClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            return new JwtClaims
            {
                NameId = jwtToken.Claims.First(c => c.Type == "nameid").Value,
                ExpirationDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jwtToken.Claims.First(c => c.Type == "exp").Value)).DateTime
            };
        }

        private static string GetJwt(JwtAuthOptions authOptions, ClaimsIdentity claims, TimeSpan exp)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(authOptions.Key);

            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Audience = authOptions.Audience,
                Issuer = authOptions.Issuer,
                Subject = claims,
                Expires = DateTime.UtcNow.Add(exp),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptior);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
