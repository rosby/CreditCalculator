using System.ComponentModel.DataAnnotations;
using CreditCalculator.Controllers.Models;
using CreditCalculator.Domain;
using DecimalMath;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CreditCalculator.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CreditController : ControllerBase
{
    private readonly CreditCalculatorService _creditCalculatorService;
    
    public CreditController(CreditCalculatorService creditCalculatorService)
    {
        _creditCalculatorService = creditCalculatorService;
    }

    [HttpPost]
    public async Task<IEnumerable<MonthlyPayment>> CalculateAsync([FromQuery]DateTime date, [FromBody]CalculateCreditApiModel model)
    {
        await Task.Yield();
        return _creditCalculatorService.CalculateAnnuityCredit(model);
        await Task.Delay(1);

        
        decimal creditBody = 150000;
        decimal percent = 10;
        decimal monthlyPercent = percent / 12 / 100;
        int numberOfMonths = 18;

        
        decimal monthlyPaymentNotRounded = creditBody * ((monthlyPercent * DecimalEx.Pow(1 + monthlyPercent, numberOfMonths))
            / (DecimalEx.Pow(1 + monthlyPercent, numberOfMonths) - 1));
        var monthlyPayment = Math.Round(monthlyPaymentNotRounded, 2);
        
        
        
    }
}