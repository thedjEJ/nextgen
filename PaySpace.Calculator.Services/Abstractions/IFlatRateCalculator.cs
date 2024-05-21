﻿using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Abstractions
{
    public interface IFlatRateCalculator
    {
        Task<CalculateResult> Calculate(decimal income);
    }
}