using PurchaseTransaction.Domain.Models.Externals.RatesExchange;

namespace PurchaseTransaction.IntegrationTests.Interfaces
{
    public interface IExchangeRateAdapter
    {
        Task<ExchangeRateDto> GetExchangeRate(string country, DateTime purchaseDate, CancellationToken cancellationToken = default);
    }
}