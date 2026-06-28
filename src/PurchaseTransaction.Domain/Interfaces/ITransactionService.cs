using PurchaseTransaction.Domain.Models;

namespace PurchaseTransaction.Domain.Interfaces
{
    public interface ITransactionService : IDisposable
    {
        Task Add(Transaction transaction);
    }
}