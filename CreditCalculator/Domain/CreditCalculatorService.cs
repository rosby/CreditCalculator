using CreditCalculator.Exceptions;
using DecimalMath;

namespace CreditCalculator.Domain;

public class CreditCalculatorService
{

    public IEnumerable<MonthlyPayment> CalculateCredit(decimal creditBody, DateTime issueDate, DateTime closingDate, decimal interestRate, СhartType chartType)
    {
        if (issueDate.Date < DateTime.Now.Date)
            throw new AppException("Кредит нельзя выдать задним числом");

        if (issueDate.Date == closingDate.Date)
            throw new AppException("День выдачи кредита может не совпадать с днем закрытия");

        if (interestRate <= 0)
            throw new AppException("Процент по ставке не может быть отрицательным или равен нулю");
        
        if(creditBody <= 0)
            throw new AppException("Сумма кредита не может быть отрицательной или равна нулю");

        
        return chartType switch
        {
            СhartType.Annuity => CalculateAnnuityCredit(creditBody, issueDate, closingDate, interestRate),
            СhartType.Differentiated => CalculateDifferentiatedCredit(creditBody, issueDate, closingDate,
                interestRate),
            _ => throw new Exception("Такой тип кредита не поддерживается")
        };
    }
    
    private IEnumerable<MonthlyPayment> CalculateDifferentiatedCredit(decimal creditAmount, DateTime issueDate, DateTime closingDate, decimal interestRate)
    {
        var paymentDates = GetPaymentDates(issueDate.Date, closingDate.Date);
        
        var creditBody = creditAmount;
        var amountOfPrincipalDebt = creditAmount / paymentDates.Count();
        var startingDateOfTheMonthlyPeriod = issueDate;
        var resultPayments = new List<MonthlyPayment>();

        foreach (var paymentDate in paymentDates)
        {
            var amountOfInterestPaymentForPeriod = GetAmountOfInterestForPeriod(creditBody, interestRate,
                startingDateOfTheMonthlyPeriod.Date, paymentDate.Date);
            var newCreditBody = creditBody - amountOfPrincipalDebt;
            
            resultPayments.Add(new MonthlyPayment
            {
                PayDate = paymentDate,
                BeginningBalance = Math.Round(creditBody, 2),
                EndingBalance = Math.Round(newCreditBody, 2),
                PrincipalRepaymentAmount = Math.Round(amountOfPrincipalDebt + amountOfInterestPaymentForPeriod, 2),
                AmountOfInterest = Math.Round(amountOfInterestPaymentForPeriod, 2),
                AmountOfPrincipalDebt = Math.Round(amountOfPrincipalDebt, 2)
            });

            creditBody = newCreditBody;
            startingDateOfTheMonthlyPeriod = paymentDate;
        }
        
        return resultPayments;
    }
    private IEnumerable<MonthlyPayment> CalculateAnnuityCredit(decimal creditAmount, DateTime issueDate, DateTime closingDate, decimal interestRate)
    {
        var paymentDates = GetPaymentDates(issueDate.Date, closingDate.Date);
        var fixedMonthlyPayment = GetMonthlyPaymentForAnnuityCredit(creditAmount, interestRate, paymentDates.Count());

        var creditBody = creditAmount;
        var startingDateOfTheMonthlyPeriod = issueDate;
        var resultPayments = new List<MonthlyPayment>();
        
        foreach (var paymentDate in paymentDates)
        {
            var amountOfInterestPaymentForPeriod = GetAmountOfInterestForPeriod(creditBody, interestRate,
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

    private IEnumerable<DateTime> GetPaymentDates(DateTime issueDate, DateTime closingDate)
    {
        if (closingDate <= issueDate)
            throw new AppException("Конечная дата не может быть больше или равна начальной");
        
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

    private decimal GetMonthlyPaymentForAnnuityCredit(decimal creditBody, decimal percent, int numberOfMonths)
    {
        var monthlyPercent = percent / 12 / 100;
        
        var monthlyPayment = creditBody * ((monthlyPercent * DecimalEx.Pow(1 + monthlyPercent, numberOfMonths))
                                                         / (DecimalEx.Pow(1 + monthlyPercent, numberOfMonths) - 1));
        return monthlyPayment;
    }
    
    private decimal GetAmountOfInterestForPeriod(decimal creditBody, decimal percent, DateTime periodStart, DateTime periodEnd)
    {
        var daysInPeriod = (int)(periodEnd.Date - periodStart.Date).TotalDays;
        var amountOfInterestForDay = creditBody * percent / 365 / 100;
        return amountOfInterestForDay * daysInPeriod;
    }
}
