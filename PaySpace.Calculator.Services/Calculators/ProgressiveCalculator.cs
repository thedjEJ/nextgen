using Microsoft.VisualBasic;
using PaySpace.Calculator.API.Models;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class ProgressiveCalculator : IProgressiveCalculator
    {

        private static TaxRate[] TaxBands = new TaxRate[]
        {
                new TaxRate(1817000m, 0.45m),
                new TaxRate(857900m, 0.41m),
                new TaxRate(673000.00m, 0.39m),
                new TaxRate(512800.00m, 0.36m),
                new TaxRate(370500.00m, 0.31m),
                new TaxRate(237100.00m, 0.26m),
                new TaxRate(0m, 0.18m),
        };
        public async Task<CalculateResult> CalculateAsync(decimal income)
        {
            decimal untaxed = income;
            decimal tax = 0;
            return await (Task.Run(() =>
            {
                foreach (TaxRate taxRate in TaxBands)
                {
                    if (untaxed > taxRate.Limit)
                    {
                        tax += (untaxed - taxRate.Limit) * taxRate.Rate;
                        untaxed = taxRate.Limit;
                    }
                }
                CalculateResult calculateResult = new CalculateResult() { Calculator = CalculatorType.Progressive, Tax = tax };

                return calculateResult;
            }));
        }
    }
}