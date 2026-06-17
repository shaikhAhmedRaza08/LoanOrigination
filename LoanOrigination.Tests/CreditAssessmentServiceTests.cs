using LoanOrigination.Application.Rules;
using LoanOrigination.Application.Services;
using LoanOrigination.Domain;
using Xunit;

namespace LoanOrigination.Tests;

/// <summary>
/// Unit tests for the credit engine. They construct the rules directly (no database,
/// no web host) which is exactly why the decision logic lives in its own layer.
/// </summary>
public class CreditAssessmentServiceTests
{
    private static CreditAssessmentService BuildService() =>
        new(new List<IRule>
        {
            new AgeRule(),
            new CreditScoreRule(),
            new FoirRule(),
            new MinimumIncomeRule()
        });

    private static LoanApplication StrongApplicant() => new()
    {
        ApplicantName = "Test",
        Age = 30,
        NetMonthlyIncome = 100000m,
        ExistingMonthlyEmi = 5000m,
        LoanAmount = 200000m,
        TenureMonths = 24,
        AnnualInterestRate = 12m,
        CreditScore = 800
    };

    [Fact]
    public void Strong_applicant_is_auto_approved()
    {
        var app = StrongApplicant();
        BuildService().Assess(app);

        Assert.Equal(DecisionStatus.Approve, app.Decision);
        Assert.Equal(ApplicationStatus.AutoApproved, app.Status);
    }

    [Fact]
    public void Low_credit_score_is_declined()
    {
        var app = StrongApplicant();
        app.CreditScore = 600;          // below 650
        BuildService().Assess(app);

        Assert.Equal(DecisionStatus.Decline, app.Decision);
    }

    [Fact]
    public void Mid_credit_score_is_referred_for_review()
    {
        var app = StrongApplicant();
        app.CreditScore = 700;          // 650-749 -> Refer, everything else passes
        BuildService().Assess(app);

        Assert.Equal(DecisionStatus.Refer, app.Decision);
        Assert.Equal(ApplicationStatus.Referred, app.Status);
    }

    [Fact]
    public void Unaffordable_loan_high_foir_is_declined()
    {
        var app = StrongApplicant();
        app.NetMonthlyIncome = 30000m;  // small income vs a large loan -> FOIR far above 60%
        app.ExistingMonthlyEmi = 15000m;
        app.LoanAmount = 500000m;
        app.TenureMonths = 12;
        BuildService().Assess(app);

        Assert.Equal(DecisionStatus.Decline, app.Decision);
        Assert.True(app.Foir > 0.60m);
    }
}
