namespace LoanOrigination.Application.Services;

/// <summary>
/// Standard reducing-balance EMI maths:
/// EMI = P * r * (1+r)^n / ((1+r)^n - 1)
/// where r = monthly interest rate and n = tenure in months.
/// </summary>
public static class EmiCalculator
{
    public static decimal CalculateEmi(decimal principal, decimal annualInterestRatePct, int tenureMonths)
    {
        if (tenureMonths <= 0 || principal <= 0)
            return 0m;

        var r = (double)(annualInterestRatePct / 100m / 12m);
        var p = (double)principal;

        // Zero-interest edge case: simple division.
        if (r == 0)
            return Math.Round(principal / tenureMonths, 2);

        var pow = Math.Pow(1 + r, tenureMonths);
        var emi = p * r * pow / (pow - 1);
        return Math.Round((decimal)emi, 2);
    }
}
