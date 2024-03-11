using Microsoft.AspNetCore.Identity;

namespace AuthService.Services.TokenProviders
{
    public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public EmailConfirmationTokenProviderOptions()
        {
            TokenLifespan = TimeSpan.FromMinutes(15);
        }
    }
}
