using LoanOrigination.Domain;

namespace LoanOrigination.Application.Rules;

/// <summary>Applicant must be within the bank's permitted working-age band.</summary>
public class AgeRule : IRule
{
    private const int MinAge = 21;
    private const int MaxAge = 60;

    public RuleResult Evaluate(LoanApplication app)
    {
        if (app.Age < MinAge || app.Age > MaxAge)
        {
            return new RuleResult(nameof(AgeRule), RuleOutcome.Fail,
                $"Applicant age {app.Age} is outside the permitted range {MinAge}-{MaxAge}.");
        }

        return new RuleResult(nameof(AgeRule), RuleOutcome.Pass,
            $"Age {app.Age} is within the permitted range {MinAge}-{MaxAge}.");
    }
}
