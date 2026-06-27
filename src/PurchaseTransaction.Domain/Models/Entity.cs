using System.ComponentModel.DataAnnotations;

namespace PurchaseTransaction.Domain.Models
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }
    }
}