namespace CreditCalculator.Domain;

public class MonthlyPayment
{
    public decimal BeginningBalance { get; set; }
    public DateTime PayDate { get; set; }
    
    public decimal PrincipalRepaymentAmount { get; set; }
    
    public decimal AmountOfPrincipalDebt { get; set; }
    
    public decimal AmountOfInterest { get; set; }
    
    public decimal EndingBalance { get; set; }
}