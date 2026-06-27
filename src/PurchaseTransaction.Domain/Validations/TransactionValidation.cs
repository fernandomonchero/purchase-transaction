using FluentValidation;
using PurchaseTransaction.Domain.Models;

namespace PurchaseTransaction.Domain.Validations
{
    public class TransactionValidation : AbstractValidator<Transaction>
    {
        public TransactionValidation()
        {
            RuleFor(t => t.Description)
                .NotEmpty().WithMessage("The {PropertyName} is required")
                .MaximumLength(50)
                .WithMessage("The {PropertyName} must not exceed {MaxLength}");

            RuleFor(t => t.Date)
                .NotEmpty()
                .WithMessage("The {PropertyName} is required");

            RuleFor(t => t.Amount)
                .NotEmpty()
                .WithMessage("The {PropertyName} is required")
                .GreaterThan(0)
                .WithMessage("The {PropertyName} must be greater than 0");
        }
    }
}