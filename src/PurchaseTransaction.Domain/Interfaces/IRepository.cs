using PurchaseTransaction.Domain.Models;

namespace PurchaseTransaction.Domain.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task Add(T entity);

        Task<T> Get(Guid id);

        Task<List<T>> All();

        Task<int> SaveChanges();
    }
}