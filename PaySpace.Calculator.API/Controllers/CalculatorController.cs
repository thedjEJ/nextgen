using MapsterMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PaySpace.Calculator.API.Models;
using PaySpace.Calculator.Data;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public sealed class CalculatorController(
        ILogger<CalculatorController> logger,
        IHistoryService historyService,
        IPostalCodeService postalCodeService,
        ICalculatorSettingsService calculatorService,
        IFlatRateCalculator flatRateCalculator,
        IFlatValueCalculator flatValueCalculator,
        IProgressiveCalculator progressiveCalculator,
        IMapper mapper)
        : ControllerBase
    {
        [HttpPost("calculate-tax")]
        public async Task<ActionResult<CalculateResult>> Calculate(CalculateRequest request)
        {
            try
            {
                
                CalculateResult result = new CalculateResult();
                PostalCode taxRate = postalCodeService.GetPostalCodesAsync().Result.Where(x => x.Code.Equals(request.PostalCode)).First();
                await calculatorService.GetSettingsAsync(taxRate.Calculator)
                    .ContinueWith(async task =>
                    {
                        if (taxRate.Calculator == CalculatorType.FlatRate)
                        {
                            result = await flatRateCalculator.Calculate(request.Income);
                        }
                        else if (taxRate.Calculator == CalculatorType.FlatValue)
                        {
                            result = await flatValueCalculator.CalculateAsync(request.Income);
                        }
                        else
                        {
                            result = await progressiveCalculator.CalculateAsync(request.Income);
                        }
                    });

                await historyService.AddAsync(new CalculatorHistory
                {
                    Tax = result.Tax,
                    Calculator = result.Calculator,
                    PostalCode = request.PostalCode ?? "Unknown",
                    Income = request.Income
                });
                

                return this.Ok(mapper.Map<CalculateResultDto>(result));
            }
            catch (CalculatorException e)
            {
                logger.LogError(e, e.Message);

                return this.BadRequest(e.Message);
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<List<CalculatorHistory>>> History()
        {
            var history = await historyService.GetHistoryAsync();

            return this.Ok(mapper.Map<List<CalculatorHistoryDto>>(history));
        }
    }
}