using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class FlatValueCalculator : IFlatValueCalculator
    {
        private readonly ICalculatorSettingsService _calculatorService;
        private readonly CalculatorSetting[] _taxBands;

        public FlatValueCalculator(ICalculatorSettingsService calculatorService)
        {
            _calculatorService = calculatorService;
            _taxBands = _calculatorService.GetSettingsAsync(CalculatorType.FlatValue).Result.ToArray();
        }
        public async Task<CalculateResult> CalculateAsync(decimal income)
        {
            CalculateResult calculateResult = new CalculateResult();
            await Task.Run(() =>
            {
                calculateResult = new CalculateResult()
            {
                Calculator = CalculatorType.FlatValue,
                Tax = (income < _taxBands.Where(x=>x.RateType.Equals(RateType.Amount)).First().From) ? 
                    income * (_taxBands.Where(x=>x.RateType.Equals(RateType.Percentage)).First().Rate/100)
                    : _taxBands.Where(x => x.RateType.Equals(RateType.Amount)).First().Rate
                };
            });

            return calculateResult;
        }
    }
}