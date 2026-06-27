using PurchaseTransaction.Api.Dtos;
using PurchaseTransaction.Domain.Models;
using PurchaseTransaction.Domain.Models.Externals.RatesExchange;

namespace PurchaseTransaction.Api.Mappings
{
    public static class ConvertedTransactionMapper
    {
        public static ConvertedTransactionDto ToDto(Transaction transaction, ExchangeRateDto exchangeRateDto)
        {
            return new ConvertedTransactionDto
            {
                Id = transaction.Id,
                Description = transaction.Description,
                PurchaseDate = transaction.Date,
                OriginalAmount = transaction.Amount,
                ExchangeRate = exchangeRateDto.ExchangeRate,
                ExchangeRateDate = exchangeRateDto.RecordDate,
                ConvertedValue = transaction.Amount * exchangeRateDto.ExchangeRate
            };
        }
    }
}