using Microsoft.EntityFrameworkCore;
using PurchaseTransaction.Domain.Models;

namespace PurchaseTransaction.Infrastructure.Contexts
{
    public class PurchaseTransactionContext : DbContext
    {
        public PurchaseTransactionContext(DbContextOptions<PurchaseTransactionContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}