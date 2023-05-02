using CreditCalculator.Controllers.Models;
using CreditCalculator.Domain;
using Microsoft.AspNetCore.Mvc;

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

    /// <summary>
    /// Рассчитать кредит
    /// </summary>
    /// <param name="model">Информация для расчета кредита</param>
    /// <returns>График платежей</returns>
    [HttpPost]
    public async Task<IEnumerable<MonthlyPayment>> CalculateAsync(
        [FromBody] CalculateCreditApiModel model)
    {
        await Task.Yield();
        return _creditCalculatorService.CalculateCredit(
            model.CreditAmount!.Value, 
            model.IssueDate!.Value, 
            model.ClosingDate!.Value, 
            model.InterestRate!.Value,
            model.СhartType!.Value);
    }
}