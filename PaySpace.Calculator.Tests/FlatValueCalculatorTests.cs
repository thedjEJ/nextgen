using Moq;
using NUnit.Framework;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    internal sealed class FlatValueCalculatorTests
    {
        private IFlatValueCalculator? _calculator;
        private Mock<ICalculatorSettingsService>? _calculatorSettingsServiceMock;

        [SetUp]
        public void Setup()
        {
            _calculatorSettingsServiceMock = new Mock<ICalculatorSettingsService>();
            _calculatorSettingsServiceMock.Setup(x => x.GetSettingsAsync(CalculatorType.FlatValue)).ReturnsAsync(new List<CalculatorSetting>
            {
                new CalculatorSetting
                { 
                    Calculator = CalculatorType.FlatValue,
                    Rate = 10000,
                    RateType = RateType.Amount,
                    From = 200000
                },
                new CalculatorSetting { 
                    Calculator = CalculatorType.FlatValue,
                    Rate = 5,
                    RateType = RateType.Percentage
                }
            });
            _calculator = new FlatValueCalculator(_calculatorSettingsServiceMock.Object);

        }

        [TestCase(199999, 9999.95)]
        [TestCase(100, 5)]
        [TestCase(200000, 10000)]
        [TestCase(6000000, 10000)]
        public async Task Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
        {
            // Arrange
            Setup();
            // Act
            var result = _calculator!.CalculateAsync(income).Result;
            decimal actualTax = result.Tax;

            // Assert
            Assert.That(actualTax, Is.EqualTo(expectedTax).Within(0.001));

        }
    }
}