using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models.Externals.RatesExchange;

namespace PurchaseTransaction.IntegrationTests.Infrastructure
{
    public class FakeExchangeRateAdapter : IExchangeRateAdapter
    {
        public IEnumerable<ExchangeRateDto> ExchangeRates { get; set; } = Enumerable.Empty<ExchangeRateDto>();

        public Exception? ExceptionToThrow { get; set; }

        public Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(
            string countryFilter,
            DateTime minDateFilter,
            DateTime maxDateFilter,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<ExchangeRateDto>>
            (
                [
                    new ExchangeRateDto
                    {
                        Country = countryFilter,
                        CountryCurrencyDescription = $"{countryFilter} Currency",
                        RecordDate = maxDateFilter,
                        EffectiveDate = maxDateFilter,
                        ExchangeRate = 5.25m
                    }
                ]
            );
        }
    }
}