using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PaySpace.Calculator.API.Models
{
    public class TaxRate
    {
        public TaxRate(decimal limit, decimal rate)
        {
            Limit = limit;
            Rate = rate;
        }

        public decimal Limit { get; set; }
        public decimal Rate { get; set; }
    }
}
