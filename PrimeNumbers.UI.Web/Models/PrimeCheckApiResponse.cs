using System.Collections.Generic;

namespace PrimeNumbers.UI.Web.Models
{
    public class PrimeCheckApiResponse
    {
        public bool IsPrime { get; set; }
        public ulong[] Primes { get; set; }
    }
}
