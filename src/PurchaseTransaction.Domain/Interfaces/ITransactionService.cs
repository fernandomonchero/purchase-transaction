using PurchaseTransaction.Domain.Models;

namespace PurchaseTransaction.Domain.Services
{
    public interface ITransactionService : IDisposable
    {
        Task Add(Transaction transaction);
    }
}