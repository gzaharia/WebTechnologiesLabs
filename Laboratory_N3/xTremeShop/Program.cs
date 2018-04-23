using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xTremeShop.Data;
using xTremeShop.Models;

namespace xTremeShop
{
    public class Program
    {
        private const string Administrator = "Administrator";

        public static void Main(string[] args)
        {
            BuildWebHost(args).MigrateDbContext<ApplicationDbContext>((context, provider) =>
            {
                var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

                var newUser = new ApplicationUser
                {
                    UserName = "gzaharia",
                    Email = "zaharia.gabi99@gmail.com",
                    SecurityStamp = string.Empty,
                };

                if (!context.Users.Any())
                {
                    var result = userManager.CreateAsync(newUser, "AlphaOmega1$").Result;

                    if (!result.Succeeded)
                    {
                        foreach (var item in result.Errors)
                        {
                            Debug.WriteLine(item.Description);
                        }
                    }
                }

                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!context.Roles.Any())
                {


                    roleManager.CreateAsync(new IdentityRole
                    {
                        Name = Administrator,
                    }).Wait();

                    var result = userManager.AddToRoleAsync(newUser, Administrator).Result;

                }
            }).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
