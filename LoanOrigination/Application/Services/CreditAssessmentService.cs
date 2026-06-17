using LoanOrigination.Application.Rules;
using LoanOrigination.Domain;

namespace LoanOrigination.Application.Services;

/// <summary>
/// The heart of the system. It (1) derives the financials the rules depend on,
/// (2) runs every registered rule, then (3) aggregates the results into one decision:
///   any Fail  -> Decline
///   else any Refer -> Refer
///   else            -> Approve
/// </summary>
public class CreditAssessmentService : ICreditAssessmentService
{
    private readonly IEnumerable<IRule> _rules;

    public CreditAssessmentService(IEnumerable<IRule> rules) => _rules = rules;

    public void Assess(LoanApplication app)
    {
        // 1. Derive EMI + FOIR (the affordability rule reads these).
        app.ProposedEmi = EmiCalculator.CalculateEmi(app.LoanAmount, app.AnnualInterestRate, app.TenureMonths);

        app.Foir = app.NetMonthlyIncome <= 0
            ? 1m
            : Math.Round((app.ExistingMonthlyEmi + app.ProposedEmi) / app.NetMonthlyIncome, 4);

        // 2. Evaluate every rule.
        var results = _rules.Select(rule => rule.Evaluate(app)).ToList();
        app.DecisionReasons = results
            .Select(r => $"[{r.Outcome}] {r.RuleName}: {r.Reason}")
            .ToList();

        // 3. Aggregate (most conservative outcome wins).
        if (results.Any(r => r.Outcome == RuleOutcome.Fail))
        {
            app.Decision = DecisionStatus.Decline;
            app.Status = ApplicationStatus.AutoDeclined;
        }
        else if (results.Any(r => r.Outcome == RuleOutcome.Refer))
        {
            app.Decision = DecisionStatus.Refer;
            app.Status = ApplicationStatus.Referred;
        }
        else
        {
            app.Decision = DecisionStatus.Approve;
            app.Status = ApplicationStatus.AutoApproved;
        }
    }
}
