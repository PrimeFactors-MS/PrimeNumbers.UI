namespace PrimeNumbers.UI.Web.Models
{
    public record PrimeDbRecord(ulong Number, bool IsPrime, ulong[] PrimeFactors);
}
