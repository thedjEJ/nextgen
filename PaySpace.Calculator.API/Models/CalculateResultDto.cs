using PaySpace.Calculator.Data.Models;

namespace PaySpace.Calculator.API.Models
{
    public sealed class CalculateResultDto
    {
        public CalculatorType Calculator { get; set; }

        public decimal Tax { get; set; }
    }
}