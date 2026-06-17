using LoanOrigination.Domain;

namespace LoanOrigination.Application.Dtos;

/// <summary>API-facing view of an application (keeps the entity out of the contract).</summary>
public class LoanApplicationResponse
{
    public Guid Id { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public decimal ProposedEmi { get; set; }
    public decimal FoirPercent { get; set; }
    public int CreditScore { get; set; }
    public string Decision { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<string> DecisionReasons { get; set; } = new();
    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public static LoanApplicationResponse From(LoanApplication a) => new()
    {
        Id = a.Id,
        ApplicantName = a.ApplicantName,
        LoanAmount = a.LoanAmount,
        ProposedEmi = a.ProposedEmi,
        FoirPercent = Math.Round(a.Foir * 100m, 2),
        CreditScore = a.CreditScore,
        Decision = a.Decision.ToString(),
        Status = a.Status.ToString(),
        DecisionReasons = a.DecisionReasons,
        ReviewedBy = a.ReviewedBy,
        ReviewedAt = a.ReviewedAt,
        CreatedAt = a.CreatedAt
    };
}
