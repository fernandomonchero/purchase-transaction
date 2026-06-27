using Microsoft.EntityFrameworkCore;
using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models;
using PurchaseTransaction.Infrastructure.Contexts;

namespace PurchaseTransaction.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly PurchaseTransactionContext Context;
        protected readonly DbSet<T> EntitySet;

        protected Repository(PurchaseTransactionContext context)
        {
            Context = context;
            EntitySet = Context.Set<T>();
        }

        public async Task Add(T entity)
        {
            EntitySet.Add(entity);

            await SaveChanges();
        }

        public async Task<T> Get(Guid id)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task<List<T>> All()
        {
            return await EntitySet.ToListAsync();
        }

        public async Task<int> SaveChanges()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}