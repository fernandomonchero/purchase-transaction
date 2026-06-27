using System.ComponentModel.DataAnnotations;

namespace PurchaseTransaction.Api.Attributes
{
    public class GreaterThanZeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
                return true;

            return value is decimal decimalValue && decimalValue > 0;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The {name} must be greater than 0";
        }
    }
}