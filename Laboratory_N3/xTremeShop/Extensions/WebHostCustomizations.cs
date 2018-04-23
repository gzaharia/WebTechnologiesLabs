using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    /// Extensions for the <see cref="IWebHost"/> instances
    /// </summary>
    public static class WebHostExtensions
    {
        /// <summary>
        /// Migrate the specified <see cref="DbContext"/> without 
        /// seeding any data to it.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="DbContext"/> to Migrate</typeparam>
        /// <param name="webHost">The <see cref="IWebHost"/> implementation where the Application will be hosted</param>
        /// <returns>The <see cref="IWebHost"/> instance to chain with other extension methods</returns>
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost) where TContext : DbContext
        {
            return MigrateDbContext<TContext>(webHost, (_, __) => { });
        }

        /// <summary>
        /// Migrate the specified <see cref="DbContext"/> applying some seed if needed
        /// </summary>
        /// <typeparam name="TContext">The Database Context to migrate</typeparam>
        /// <param name="webHost">The <see cref="IWebHost"/> the dbcontext is running on</param>
        /// <param name="seeder">A seeder function to seed some custom data into the context</param>
        /// <returns>The <see cref="IWebHost"/> instance to chain with other extension methods</returns>
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();
                var context = serviceProvider.GetService<TContext>();
                string contextTName = typeof(TContext).Name;

                try
                {
                    logger.LogInformation($"Migrating database associated with context {contextTName}");

                    context.Database.Migrate();

                    seeder(context, serviceProvider);
                    logger.LogInformation($"Migrated database associated with context {contextTName}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occured while migrating the database used on context {contextTName}");
                }
            }

            return webHost;
        }
    }

}
