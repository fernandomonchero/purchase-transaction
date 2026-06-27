using PurchaseTransaction.Domain.Models.Externals.RatesExchange;

namespace PurchaseTransaction.Domain.Interfaces
{
    public interface IExchangeRateAdapter
    {
        Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(string countryFilter, DateTime minDateFilter, DateTime maxDateFilter, CancellationToken cancellationToken = default);
    }
}