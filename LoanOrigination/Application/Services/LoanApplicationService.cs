using LoanOrigination.Application.Dtos;
using LoanOrigination.Domain;
using LoanOrigination.Infrastructure.Data;

namespace LoanOrigination.Application.Services;

/// <summary>Orchestrates the use cases: submit, fetch, and the maker-checker review.</summary>
public class LoanApplicationService : ILoanApplicationService
{
    private readonly ILoanApplicationRepository _repo;
    private readonly ICreditAssessmentService _assessment;

    public LoanApplicationService(ILoanApplicationRepository repo, ICreditAssessmentService assessment)
    {
        _repo = repo;
        _assessment = assessment;
    }

    public async Task<LoanApplicationResponse> SubmitAsync(CreateLoanApplicationRequest request)
    {
        var application = new LoanApplication
        {
            ApplicantName = request.ApplicantName,
            Age = request.Age,
            NetMonthlyIncome = request.NetMonthlyIncome,
            ExistingMonthlyEmi = request.ExistingMonthlyEmi,
            LoanAmount = request.LoanAmount,
            TenureMonths = request.TenureMonths,
            AnnualInterestRate = request.AnnualInterestRate <= 0 ? 12m : request.AnnualInterestRate,
            CreditScore = request.CreditScore
        };

        // Run underwriting, then persist the result.
        _assessment.Assess(application);
        await _repo.AddAsync(application);

        return LoanApplicationResponse.From(application);
    }

    public async Task<LoanApplicationResponse?> GetAsync(Guid id)
    {
        var app = await _repo.GetByIdAsync(id);
        return app is null ? null : LoanApplicationResponse.From(app);
    }

    public async Task<List<LoanApplicationResponse>> GetAllAsync()
    {
        var apps = await _repo.GetAllAsync();
        return apps.Select(LoanApplicationResponse.From).ToList();
    }

    public async Task<(bool ok, string? error, LoanApplicationResponse? result)> ReviewAsync(Guid id, ReviewRequest request)
    {
        var app = await _repo.GetByIdAsync(id);
        if (app is null)
            return (false, "Application not found.", null);

        // Maker-checker only applies to cases the engine referred for human review.
        if (app.Status != ApplicationStatus.Referred)
            return (false, $"Only referred applications can be reviewed. Current status: {app.Status}.", null);

        app.Status = request.Approve ? ApplicationStatus.ReviewApproved : ApplicationStatus.ReviewRejected;
        app.Decision = request.Approve ? DecisionStatus.Approve : DecisionStatus.Decline;
        app.ReviewedBy = request.ReviewedBy;
        app.ReviewedAt = DateTime.UtcNow;
        app.ReviewComment = request.Comment;
        app.DecisionReasons.Add(
            $"[Review] {(request.Approve ? "Approved" : "Rejected")} by {request.ReviewedBy}" +
            (string.IsNullOrWhiteSpace(request.Comment) ? "" : $" - {request.Comment}"));

        await _repo.SaveChangesAsync();
        return (true, null, LoanApplicationResponse.From(app));
    }
}
