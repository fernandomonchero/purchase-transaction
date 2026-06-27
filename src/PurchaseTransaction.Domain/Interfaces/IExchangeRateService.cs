using PurchaseTransaction.Domain.Models.Externals.RatesExchange;

namespace PurchaseTransaction.Domain.Interfaces
{
    public interface IExchangeRateService
    {
        Task<ExchangeRateDto?> GetValidExchangeRate(string country, DateTime purchaseDate);
    }
}