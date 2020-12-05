using System.Collections.Generic;

namespace PrimeNumbers.UI.Web.Models
{
    public class PrimeCheckResponse
    {
        public bool IsPrime { get; set; }
        public ulong[] Primes { get; set; }
        public string ErrorMessage { get; set; }
    }
}
