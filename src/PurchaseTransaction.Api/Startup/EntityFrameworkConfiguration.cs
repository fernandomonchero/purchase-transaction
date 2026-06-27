using Microsoft.EntityFrameworkCore;
using PurchaseTransaction.Infrastructure.Contexts;

namespace PurchaseTransaction.Api.Startup
{
    public static class EntityFrameworkConfiguration
    {
        public static IServiceCollection ConfigureEntityFramework(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PurchaseTransactionContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }

        public static async Task<WebApplication> UseEntityFramework(this WebApplication webApplication)
        {
            var retries = 10;

            while (retries > 0)
            {
                try
                {
                    using var scope = webApplication.Services.CreateScope();

                    var context = scope.ServiceProvider.GetRequiredService<PurchaseTransactionContext>();

                    context.Database.Migrate();

                    break;
                }
                catch
                {
                    retries--;

                    if (retries == 0)
                        throw;

                    await Task.Delay(5000);
                }
            }

            return webApplication;
        }
    }
}