using PurchaseTransaction.Domain.Models;

namespace PurchaseTransaction.Domain.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
    }
}