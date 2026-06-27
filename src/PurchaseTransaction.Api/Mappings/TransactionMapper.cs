using PurchaseTransaction.Api.Dtos;
using PurchaseTransaction.Domain.Extensions;
using PurchaseTransaction.Domain.Models;

namespace PurchaseTransaction.Api.Mappings
{
    public static class TransactionMapper
    {
        public static Transaction ToEntity(TransactionDto transactionDto)
        {
            return new Transaction
            {
                Description = transactionDto.Description,
                Date = transactionDto.Date,
                Amount = transactionDto.Amount.Truncate()
            };
        }

        public static TransactionDto ToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Date = transaction.Date,
                Amount = transaction.Amount
            };
        }
    }
}