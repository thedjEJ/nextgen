using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using PaySpace.Calculator.Data;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;


namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    internal sealed class FlatRateCalculatorTests
    {
        private IFlatRateCalculator? _calculator;
        private Mock<ICalculatorSettingsService>? _calculatorSettingsServiceMock;

        [SetUp]
        public void Setup()
        {
            _calculatorSettingsServiceMock = new Mock<ICalculatorSettingsService>();
            _calculatorSettingsServiceMock.Setup(x => x.GetSettingsAsync(CalculatorType.FlatRate)).ReturnsAsync(new List<CalculatorSetting>
            {
                new CalculatorSetting
                {
                    Rate = 17.5m,
                    RateType = RateType.Percentage
                }
            });
            _calculator = new FlatRateCalculator(_calculatorSettingsServiceMock.Object);
        }

        [TestCase(999999, 174999.825)]
        [TestCase(1000, 175)]
        [TestCase(5, 0.875)]
        public void Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
        {
            // Arrange
            Setup();
            // Act
            var result = _calculator!.Calculate(income).Result;
            decimal actualTax = result.Tax;

            // Assert
            Assert.That(actualTax, Is.EqualTo(expectedTax).Within(0.001));
        }
    }
}
