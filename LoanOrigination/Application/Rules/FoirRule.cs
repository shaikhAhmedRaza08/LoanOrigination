using LoanOrigination.Domain;

namespace LoanOrigination.Application.Rules;

/// <summary>
/// FOIR (Fixed Obligation to Income Ratio) is the core affordability check:
/// (existing EMIs + proposed EMI) / net monthly income.
/// &lt;=50% is comfortable, 50-60% is borderline (Refer), &gt;60% is unaffordable (Fail).
/// </summary>
public class FoirRule : IRule
{
    private const decimal PassThreshold = 0.50m;
    private const decimal ReferThreshold = 0.60m;

    public RuleResult Evaluate(LoanApplication app)
    {
        var pct = app.Foir * 100m;

        if (app.Foir <= PassThreshold)
            return new RuleResult(nameof(FoirRule), RuleOutcome.Pass,
                $"FOIR {pct:F1}% is within the safe affordability limit ({PassThreshold * 100:F0}%).");

        if (app.Foir <= ReferThreshold)
            return new RuleResult(nameof(FoirRule), RuleOutcome.Refer,
                $"FOIR {pct:F1}% exceeds {PassThreshold * 100:F0}% and needs underwriter review.");

        return new RuleResult(nameof(FoirRule), RuleOutcome.Fail,
            $"FOIR {pct:F1}% exceeds the maximum affordability limit ({ReferThreshold * 100:F0}%).");
    }
}
