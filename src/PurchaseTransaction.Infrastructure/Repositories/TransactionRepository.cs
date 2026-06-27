using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models;
using PurchaseTransaction.Infrastructure.Contexts;

namespace PurchaseTransaction.Infrastructure.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(PurchaseTransactionContext context) : base(context)
        {
        }
    }
}