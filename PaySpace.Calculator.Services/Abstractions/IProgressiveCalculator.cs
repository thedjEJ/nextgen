using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Abstractions
{
    public interface IProgressiveCalculator
    {
        public Task<CalculateResult> CalculateAsync(decimal income);
    }
}