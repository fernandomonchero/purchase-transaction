namespace PurchaseTransaction.Domain.Models
{
    public class Transaction : Entity
    {
        public string Description { get; set; } = "";

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}