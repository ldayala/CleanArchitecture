﻿using CleanArchitecture.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CleanArchitecture.Api.OptionsSetup
{
    public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtOptions;

        public JwtBearerOptionsSetup(IOptions<JwtOptions>jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            ConfigureToken(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            throw new NotImplementedException();
        }

        private void ConfigureToken(JwtBearerOptions options) {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey!)),
            };
        }
    }
}
