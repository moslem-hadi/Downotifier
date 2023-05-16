using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var oidc = "oidc";
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddControllers();


        //services.AddAuthentication(
        //        options =>
        //        {
        //            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //            options.DefaultChallengeScheme = oidc;
        //        }
        //    )
        //    .AddOpenIdConnect(oidc , options =>
        //        {
        //            options.GetClaimsFromUserInfoEndpoint = true;
        //            options.Authority = configuration["ServiceUrls:IdentityApi"];

        //            options.ClientId = configuration["OpenIdConnect:ClientId"];
        //            options.ClientSecret = configuration["OpenIdConnect:ClientSecret"];
        //            options.ResponseType = configuration["OpenIdConnect:ResponseType"]!;

        //            options.TokenValidationParameters.NameClaimType = "name";
        //            options.TokenValidationParameters.RoleClaimType = "role";
        //            options.Scope.Add(configuration["OpenIdConnect:Scope"]!);
        //            options.SaveTokens = true;
        //            options.ClaimActions.MapJsonKey("role","role");
        //        }
        //    );

        var key = configuration["OpenIdConnect:ClientSecret"]!;
        var authority = configuration["ServiceUrls:IdentityApi"]!;

        services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(opstions =>
            {
                opstions.RequireHttpsMetadata = false;
                opstions.SaveToken = true;
                opstions.Authority = authority;
                opstions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("ApiScope", policy =>
        //    {
        //        policy.RequireAuthenticatedUser();
        //        policy.RequireClaim("scope", "apiJobsScope");
        //    });
        //});

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
