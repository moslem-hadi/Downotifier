﻿using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

         services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddControllers();


        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Api",
                Version = "v1"
            });
            //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            //{
            //    Name = "Authorization",
            //    Type = SecuritySchemeType.ApiKey,
            //    Scheme = "Bearer",
            //    BearerFormat = "JWT",
            //    In = ParameterLocation.Header,
            //    Description = "JWT Authorization header using the Bearer scheme. " +
            //    "\r\n\r\n Enter 'Bearer [space] <your token>' in the text input below." +
            //    "\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6.....\"",
            //});
            //c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            //    {
            //        new OpenApiSecurityScheme {
            //            Reference = new OpenApiReference {
            //                Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //            }
            //        },
            //        new string[] {}
            //    }
            //});
        });

        return services;
    }
}
