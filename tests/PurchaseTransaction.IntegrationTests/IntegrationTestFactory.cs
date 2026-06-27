using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PurchaseTransaction.Infrastructure.Contexts;

namespace PurchaseTransaction.IntegrationTests
{
    public class IntegrationTestFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<PurchaseTransactionContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<PurchaseTransactionContext>(options =>
                {
                    options.UseSqlServer(
                        @"Server=(localdb)\MSSQLLocalDB;
                      Database=PurchaseTransactionTest;
                      Trusted_Connection=True;
                      TrustServerCertificate=True");
                });

                var provider = services.BuildServiceProvider();

                using var scope = provider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<PurchaseTransactionContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            });
        }

        public IServiceScope CreateScope()
        {
            return Services.CreateScope();
        }
    }
}