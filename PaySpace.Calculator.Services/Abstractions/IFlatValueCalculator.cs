using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Abstractions
{
    public interface IFlatValueCalculator
    {
        Task<CalculateResult> CalculateAsync(decimal income);
    }
}