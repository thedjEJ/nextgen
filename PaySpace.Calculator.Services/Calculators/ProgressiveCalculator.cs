using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class ProgressiveCalculator : IProgressiveCalculator
    {
        private readonly ICalculatorSettingsService _calculatorService;
        private readonly CalculatorSetting[] _taxBands;

        public ProgressiveCalculator(ICalculatorSettingsService calculatorService)
        {
            _calculatorService = calculatorService;
            _taxBands = _calculatorService.GetSettingsAsync(CalculatorType.Progressive).Result.ToArray();
        }

        public async Task<CalculateResult> CalculateAsync(decimal income)
        {
            decimal untaxed = income;
            decimal tax = 0;

            await Task.Run(() =>
            {
            foreach (CalculatorSetting taxRate in _taxBands.Where(x => x.Calculator.Equals(CalculatorType.Progressive)).OrderByDescending(x => x.From))
            {
                if (untaxed > (taxRate.From))
                {
                    decimal taxBracketPortion = (decimal)(untaxed - (taxRate.From));
                    tax += (taxBracketPortion) * (taxRate.Rate / 100.0m);
                    untaxed = taxRate.From-1;
                }
            }
            });

            CalculateResult calculateResult = new CalculateResult()
            {
                Calculator = CalculatorType.Progressive,
                Tax = tax
            };

            return calculateResult;
        }
    }
}