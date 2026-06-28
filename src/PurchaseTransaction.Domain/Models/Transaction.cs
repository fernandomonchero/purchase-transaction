using System.ComponentModel.DataAnnotations;

namespace PurchaseTransaction.Domain.Models
{
    public class Transaction : Entity
    {
        [MaxLength(50)]
        public string Description { get; set; } = "";

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}