using Moq;
using NUnit.Framework;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    internal sealed class ProgressiveCalculatorTests
    {
        private IProgressiveCalculator? _calculator;
        private Mock<ICalculatorSettingsService>? _calculatorSettingsServiceMock;
        [SetUp]
        public void Setup()
        {
            _calculatorSettingsServiceMock = new Mock<ICalculatorSettingsService>();
            _calculatorSettingsServiceMock.Setup(x => x.GetSettingsAsync(CalculatorType.Progressive)).ReturnsAsync(new List<CalculatorSetting>
            {
                                     
                new CalculatorSetting
                {
                        Calculator = CalculatorType.Progressive,
                        Rate = 10.00m,
                        RateType = RateType.Percentage,
                        From = 0.00m,
                        To = 8350.00m
                },
                new CalculatorSetting
                {
                        Calculator = CalculatorType.Progressive,
                        Rate = 15.00m,
                        RateType = RateType.Percentage,
                        From = 8351.00m,
                        To = 33950.00m
                },
                new CalculatorSetting
                {
                        Calculator = CalculatorType.Progressive,
                        Rate = 25.00m,
                        RateType = RateType.Percentage,
                        From = 33951.00m,
                        To = 82250.00m
                },
                new CalculatorSetting
                {
                        Calculator = CalculatorType.Progressive,
                        Rate = 28.00m,
                        RateType = RateType.Percentage,
                        From = 82251.00m,
                        To = 171550.00m
                },
                new CalculatorSetting
                {
                        Calculator = CalculatorType.Progressive,
                        Rate = 33.00m,
                        RateType = RateType.Percentage,
                        From = 171551.00m,
                        To = 372950.00m
                },
                new CalculatorSetting
                {
                        Calculator = CalculatorType.Progressive,
                        Rate = 35.00m,
                        RateType = RateType.Percentage,
                        From = 372951.00m,
                        To = 0.00m
                }
            });
            _calculator = new ProgressiveCalculator(_calculatorSettingsServiceMock.Object);

        }

        [TestCase(-1, 0)]
        [TestCase(50, 5)]
        [TestCase(8350.1, 835.01)]
        [TestCase(8351, 835)] // Pretty sure this should be 835.15
        [TestCase(33951, 4674.85)] // Pretty sure this should be 4674.75
        [TestCase(82251, 16749.60)] // Not sure why this fails... yet...
        [TestCase(171550, 41753.32)]
        [TestCase(999999, 327681.79)]
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