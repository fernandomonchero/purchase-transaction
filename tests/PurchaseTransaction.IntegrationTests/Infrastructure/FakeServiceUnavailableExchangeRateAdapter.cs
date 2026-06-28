using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models.Externals.RatesExchange;

namespace PurchaseTransaction.IntegrationTests.Infrastructure
{
    public class ServiceUnavailableExchangeRateAdapter : IExchangeRateAdapter
    {
        public Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(string countryFilter, DateTime minDateFilter, DateTime maxDateFilter, CancellationToken cancellationToken = default)
        {
            throw new HttpRequestException();
        }
    }
}