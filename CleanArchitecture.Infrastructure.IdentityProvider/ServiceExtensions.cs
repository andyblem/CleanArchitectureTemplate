using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Wrappers;
using CleanArchitecture.Infrastructure.IdentityProvider.DTOs;
using CleanArchitecture.Infrastructure.IdentityProvider.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace CleanArchitecture.Infrastructure.IdentityProvider
{
    public static class ServiceExtensions
    {
        public static void AddIdentityProviderInfrastructure(this IServiceCollection services, WebApplicationBuilder builder, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("IdentityProvider")));

            // load data from settings to DI
            builder.Services.Configure<JwtSettingsDTO>(configuration.GetSection("JWTSettings"));

            // prepare jwt data
            var jwtSettingsSection = configuration.GetSection("JWTSettings");
            var jwtSettings = jwtSettingsSection.Get<JwtSettingsDTO>();
            var issuerSigningKey = Encoding.ASCII.GetBytes(jwtSettings.Key);

            //Authentication Middleware(sets the contitions to be checked and be true for user to be authenticated)
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(issuerSigningKey)
                };
            });

           
            services.AddTransient<IAccountService, AccountService>();
        }
    }
}
