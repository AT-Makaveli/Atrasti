using System;
using Atrasti.API.Models.Config;
using Atrasti.API.Services.Jwt;
using Atrasti.API.Services.Refresh;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Atrasti.API.Services
{
    public static class ServiceInitializer
    {
        public static void InitAtrastiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICountryInfoService, CountryInfoService>();
            
            services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
            services.AddScoped<IEmailSender, EmailSender>();
            
            services.AddScoped<IRefreshService, RefreshService>();
            services.AddScoped<IJwtService, JwtService>();
        }

        public static void InitAtrastiAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var bindJwtSettings = new JwtSettings();
            configuration.Bind("JsonWebTokenKeys", bindJwtSettings);
            services.AddSingleton(bindJwtSettings);
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters() {
                    ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
                    ValidateIssuer = bindJwtSettings.ValidateIssuer,
                    ValidIssuer = bindJwtSettings.ValidIssuer,
                    ValidateAudience = bindJwtSettings.ValidateAudience,
                    ValidAudience = bindJwtSettings.ValidAudience,
                    RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
                    ValidateLifetime = bindJwtSettings.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromDays(1),
                };
            });
        }
    }
}