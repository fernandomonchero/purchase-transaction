using PurchaseTransaction.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PurchaseTransaction.Api.Dtos
{
    public class TransactionDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        [MaxLength(50, ErrorMessage = "The {0} must not exceed {1}")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        [GreaterThanZero]
        public decimal Amount { get; set; }
    }
}