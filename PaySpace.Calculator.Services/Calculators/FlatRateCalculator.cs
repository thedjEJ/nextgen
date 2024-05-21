using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class FlatRateCalculator : IFlatRateCalculator
    {
        private readonly ICalculatorSettingsService _calculatorService;
        private readonly CalculatorSetting[] _taxBands;

        public FlatRateCalculator(ICalculatorSettingsService calculatorService)
        {
            _calculatorService = calculatorService;
            _taxBands = _calculatorService.GetSettingsAsync(CalculatorType.FlatRate).Result.ToArray();
        }

        async Task<CalculateResult> IFlatRateCalculator.Calculate(decimal income)
        {
            CalculateResult calculateResult = new CalculateResult();
            await Task.Run(() =>
            {
                calculateResult = new CalculateResult()
                {
                    Calculator = CalculatorType.FlatRate,
                    Tax = income * (_taxBands.Where(x => x.RateType.Equals(RateType.Percentage)).First().Rate / 100)
                };
            });

            return calculateResult;
        }
    }
}