using LoanOrigination.Domain;

namespace LoanOrigination.Application.Rules;

/// <summary>
/// CIBIL-based risk band: 750+ is clean, 650-749 needs a human look (Refer),
/// below 650 is auto-declined.
/// </summary>
public class CreditScoreRule : IRule
{
    private const int GoodScore = 750;
    private const int MinReferScore = 650;

    public RuleResult Evaluate(LoanApplication app)
    {
        if (app.CreditScore >= GoodScore)
            return new RuleResult(nameof(CreditScoreRule), RuleOutcome.Pass,
                $"CIBIL score {app.CreditScore} meets the good-credit threshold ({GoodScore}+).");

        if (app.CreditScore >= MinReferScore)
            return new RuleResult(nameof(CreditScoreRule), RuleOutcome.Refer,
                $"CIBIL score {app.CreditScore} is in the manual-review band ({MinReferScore}-{GoodScore - 1}).");

        return new RuleResult(nameof(CreditScoreRule), RuleOutcome.Fail,
            $"CIBIL score {app.CreditScore} is below the minimum acceptable ({MinReferScore}).");
    }
}
