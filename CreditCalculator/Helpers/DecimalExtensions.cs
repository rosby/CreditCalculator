namespace CreditCalculator.Helpers;

public static class DecimalExtensions
{
    public static decimal Pow(this decimal number, int degree)
    {
        switch (degree)
        {
            case 0:
                return 1;
            case 1:
                return number;
        }

        var initialNumber = number;
        var finalNumber = number;
        
        if(degree < 0)
        for (var i = 1; i < degree; i++)
        {
            finalNumber *= initialNumber;
        }

        return finalNumber;
    }
}