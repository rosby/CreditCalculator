using CreditCalculator.Controllers.Models;
using DecimalMath;

namespace CreditCalculator.Domain;

public class CreditCalculatorService
{

    
    public IEnumerable<MonthlyPayment> CalculateAnnuityCredit(CalculateCreditApiModel model)
    {
        var paymentDates = GetPaymentDates(model.IssueDate.Date, model.ClosingDate.Date);
        var fixedMonthlyPayment = GetMonthlyPaymentForAnnuityCredit(model.SumOfMoney, model.InterestRate, paymentDates.Count());

        var creditBody = model.SumOfMoney;
        var startingDateOfTheMonthlyPeriod = model.IssueDate;
        var resultPayments = new List<MonthlyPayment>();
        
        foreach (var paymentDate in paymentDates)
        {
            var amountOfInterestPaymentForPeriod = GetAmountOfInterestForPeriod(creditBody, model.InterestRate,
                startingDateOfTheMonthlyPeriod.Date, paymentDate.Date);

            var freeAmountForPrincipal = fixedMonthlyPayment - amountOfInterestPaymentForPeriod;

            var amountOfPrincipalDebt = freeAmountForPrincipal <= creditBody ? freeAmountForPrincipal : creditBody;
            var newCreditBody = creditBody - amountOfPrincipalDebt;
            resultPayments.Add(new MonthlyPayment
            {
                BeginningBalance = Math.Round(creditBody, 2),
                PayDate = paymentDate,
                PrincipalRepaymentAmount = Math.Round(amountOfPrincipalDebt + amountOfInterestPaymentForPeriod, 2),
                AmountOfInterest = Math.Round(amountOfInterestPaymentForPeriod, 2),
                AmountOfPrincipalDebt = Math.Round(amountOfPrincipalDebt, 2),
                EndingBalance = Math.Round(newCreditBody, 2)
            });

            startingDateOfTheMonthlyPeriod = paymentDate;
            creditBody = newCreditBody;
        }
        
        return resultPayments;
    }

    public IEnumerable<DateTime> GetPaymentDates(DateTime issueDate, DateTime closingDate)
    {
        var paymentDates = new List<DateTime>();

        var numberOfMonth = 1;
        var working = true;
        while(working)
        {
            var paymentDate = issueDate.AddMonths(numberOfMonth);
            if (paymentDate.Date == closingDate)
            {
                working = false;
            }
            else if (paymentDate.Date > closingDate)
            {
                paymentDates.Add(new DateTime(closingDate.Year, closingDate.Month, closingDate.Day));
                break;
            }
            
            paymentDates.Add(paymentDate);
            numberOfMonth++;
        }

        return paymentDates;
    }

    public decimal GetMonthlyPaymentForAnnuityCredit(decimal creditBody, decimal percent, int numberOfMonths)
    {
        var monthlyPercent = percent / 12 / 100;
        
        var monthlyPayment = creditBody * ((monthlyPercent * DecimalEx.Pow(1 + monthlyPercent, numberOfMonths))
                                                         / (DecimalEx.Pow(1 + monthlyPercent, numberOfMonths) - 1));
        return monthlyPayment;
    }
    
    public decimal GetAmountOfInterestForPeriod(decimal creditBody, decimal percent, DateTime periodStart, DateTime periodEnd)
    {
        var daysInPeriod = (int)(periodEnd.Date - periodStart.Date).TotalDays;
        var amountOfInterestForDay = creditBody * percent / 365 / 100;
        return amountOfInterestForDay * daysInPeriod;
    }
}
