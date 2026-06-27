
namespace PurchaseTransaction.Api.Dtos
{
    public class ConvertedTransactionDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal OriginalAmount { get; set; }

        public DateTime ExchangeRateDate { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal ConvertedValue { get; set; }
    }
}