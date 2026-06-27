namespace PurchaseTransaction.Domain.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal Truncate(this decimal value)
        {
            decimal factor = (decimal)Math.Pow(10, 2);
            return Math.Truncate(value * factor) / factor;
        }
    }
}