﻿using CleanArchitecture.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Api.OptionsSetup
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private const string SectionName = "Jwt";
        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
           _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
