using System.Security.Claims;
using IdentityModel;
using IdentityProvider.Data;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityProvider;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (roleManager.FindByNameAsync(Config.Constants.UserRole).GetAwaiter().GetResult() == null)
            {
                roleManager.CreateAsync(new IdentityRole(Config.Constants.UserRole)).GetAwaiter().GetResult();
            }


            var user = userManager.FindByNameAsync("user").GetAwaiter().GetResult();
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "user",
                    Email = "user@mail.com",
                    EmailConfirmed = true,
                };
                var result = userManager.CreateAsync(user, "Pass123!").GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                userManager.AddToRoleAsync(user, Config.Constants.UserRole).GetAwaiter().GetResult();

                result = userManager.AddClaimsAsync(user,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.Role, Config.Constants.UserRole),
                            new Claim(JwtClaimTypes.Name, user.UserName),
                        }).GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("default user created");
            }
            else
            {
                Log.Debug("default user already exists");
            } 
        }
    }
}
