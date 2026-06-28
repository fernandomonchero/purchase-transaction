using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models;
using PurchaseTransaction.Infrastructure.Contexts;


namespace PurchaseTransaction.IntegrationTests.Infrastructure
{
    public class IntegrationTestFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection? _connection;

        private readonly Type _exchangeRateAdapterType;

        public IntegrationTestFactory(Type? exchangeRateAdapterType = null)
        {
            _exchangeRateAdapterType = exchangeRateAdapterType ?? typeof(FakeExchangeRateAdapter);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<PurchaseTransactionContext>>();
                services.RemoveAll<SqliteConnection>();
                services.RemoveAll<IExchangeRateAdapter>();

                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                services.AddSingleton(_connection);
                services.AddDbContext<PurchaseTransactionContext>(options =>
                {
                    options.UseSqlite(_connection);
                });

                services.RemoveAll<IExchangeRateAdapter>();
                services.AddScoped(typeof(IExchangeRateAdapter), _exchangeRateAdapterType);

                var provider = services.BuildServiceProvider();

                using var scope = provider.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<PurchaseTransactionContext>();
                context.Database.Migrate();

                SeedDatabase(context);
            });
        }

        private static void SeedDatabase(PurchaseTransactionContext context)
        {
            context.Transactions.Add(new Transaction
            {
                Id = new Guid("0ea39c29-580e-4a80-a4c9-798b9e31fd0e"),
                Description = "Red Shirt",
                Amount = 10,
                Date = new DateTime(2026, 1, 1)
            });

            context.SaveChanges();
        }

        public PurchaseTransactionContext CreateDbContext()
        {
            var scope = Services.CreateScope();

            return scope.ServiceProvider.GetRequiredService<PurchaseTransactionContext>();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _connection?.Dispose();
        }
    }
}