using CreditCalculator.Domain;

namespace CreditCalculator.Controllers.Models;

public class CalculateCreditApiModel
{
    public decimal SumOfMoney { get; set; }
    
    public DateTime IssueDate { get; set; }
    
    public DateTime ClosingDate { get; set; }
    
    public decimal InterestRate { get; set; }
    
    public СhartType СhartType { get; set; }
}