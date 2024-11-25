﻿using API.Services;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityService(this IServiceCollection service, IConfiguration config)
    {
        service.AddIdentityCore<AppUser>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<DataContext>();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                    {
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = key,
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

        service.AddAuthorization(opt =>
        {
            opt.AddPolicy("IsActivityHost", policy =>
            {
                policy.Requirements.Add(new IsHostRequirement());
            });
        });
        service.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
        service.AddScoped<TokenService>();

        return service;
    }
}
