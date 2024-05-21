using PaySpace.Calculator.Data.Models;

namespace PaySpace.Calculator.API.Models
{
    public sealed class PostalCodeDto
    {
        public string? Code { get; set; }

        public CalculatorType Calculator { get; set; }
    }
}