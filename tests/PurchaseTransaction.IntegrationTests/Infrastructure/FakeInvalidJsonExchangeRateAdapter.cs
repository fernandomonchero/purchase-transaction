using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models.Externals.RatesExchange;
using System.Text.Json;

namespace PurchaseTransaction.IntegrationTests.Infrastructure
{
    public class InvalidJsonExchangeRateAdapter : IExchangeRateAdapter
    {
        public Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(string countryFilter, DateTime minDateFilter, DateTime maxDateFilter, CancellationToken cancellationToken = default)
        {
            throw new JsonException();
        }
    }
}