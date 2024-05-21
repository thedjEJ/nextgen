using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using PaySpace.Calculator.Data;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.Services
{
    internal sealed class CalculatorSettingsService : ICalculatorSettingsService
    {
        private readonly CalculatorContext context;
        private readonly IMemoryCache memoryCache;

        public CalculatorSettingsService(CalculatorContext context, IMemoryCache memoryCache)
        {
            this.context = context;
            this.memoryCache = memoryCache;
        }

        public Task<List<CalculatorSetting>> GetSettingsAsync(CalculatorType calculatorType)
        {
            return memoryCache.GetOrCreateAsync($"CalculatorSetting:{calculatorType}", entry =>
            {
                return context.Set<CalculatorSetting>().AsNoTracking().Where(_ => _.Calculator == calculatorType).ToListAsync();
            })!;
        }
    }
}