﻿using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AuthService.Services.TokenProviders
{
    public class EmailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser>
    where TUser : class
    {
        public EmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider, 
            IOptions<DataProtectionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) 
            : base(dataProtectionProvider, options, logger)
        {
        }
    }
}
