using PaySpace.Calculator.Data.Models;

namespace PaySpace.Calculator.API.Models
{
    public sealed class CalculatorHistoryDto
    {
        public string? PostalCode { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal Income { get; set; }

        public decimal Tax { get; set; }

        public CalculatorType Calculator { get; set; }
    }
}