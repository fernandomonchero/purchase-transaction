using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Notifications;
using PurchaseTransaction.Domain.Services;
using PurchaseTransaction.Infrastructure.Adapters;
using PurchaseTransaction.Infrastructure.Repositories;

namespace PurchaseTransaction.Api.Startup
{
    public static class DependencyInjectionSolver
    {
        public static IServiceCollection SolveDependencies(this IServiceCollection services, string urlTreasuryApi)
        {
            services.AddScoped<INotificationCollector, NotificationCollector>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IExchangeRateService, ExchangeRateService>();

            services.AddHttpClient<IExchangeRateAdapter, ExchangeRateAdapter>(client =>
            {
                client.BaseAddress = new Uri(urlTreasuryApi);
                client.Timeout = TimeSpan.FromSeconds(15);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddStandardResilienceHandler();

            return services;
        }
    }
}