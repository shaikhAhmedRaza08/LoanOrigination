using LoanOrigination.Domain;

namespace LoanOrigination.Application.Rules;

/// <summary>Applicant must clear the product's minimum net monthly income.</summary>
public class MinimumIncomeRule : IRule
{
    private const decimal MinIncome = 25000m;

    public RuleResult Evaluate(LoanApplication app)
    {
        if (app.NetMonthlyIncome < MinIncome)
            return new RuleResult(nameof(MinimumIncomeRule), RuleOutcome.Fail,
                $"Net monthly income {app.NetMonthlyIncome:N0} is below the minimum required ({MinIncome:N0}).");

        return new RuleResult(nameof(MinimumIncomeRule), RuleOutcome.Pass,
            $"Net monthly income {app.NetMonthlyIncome:N0} meets the minimum requirement ({MinIncome:N0}).");
    }
}
