using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models.Externals.RatesExchange;

namespace PurchaseTransaction.Domain.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateAdapter _exchangeRateAdapter;

        public ExchangeRateService(IExchangeRateAdapter exchangeRateAdapter)
        {
            _exchangeRateAdapter = exchangeRateAdapter;
        }

        public async Task<ExchangeRateDto?> GetValidExchangeRate(string country, DateTime purchaseDate)
        {
            var minDateFilter = purchaseDate.AddMonths(-6);

            var exchangeRates = await _exchangeRateAdapter.GetExchangeRates(country, minDateFilter, purchaseDate);

            return exchangeRates
                .Where(x => x.RecordDate >= minDateFilter && x.RecordDate <= purchaseDate)
                .OrderByDescending(x => x.RecordDate)
                .FirstOrDefault();
        }
    }
}