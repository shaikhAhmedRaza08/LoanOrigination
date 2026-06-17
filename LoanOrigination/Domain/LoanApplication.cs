namespace LoanOrigination.Domain;

/// <summary>
/// The core entity. Holds the applicant's inputs, the values the engine derives
/// (EMI, FOIR), and the decision/audit trail.
/// </summary>
public class LoanApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // ----- Applicant inputs -----
    public string ApplicantName { get; set; } = string.Empty;
    public int Age { get; set; }
    public decimal NetMonthlyIncome { get; set; }
    public decimal ExistingMonthlyEmi { get; set; }
    public decimal LoanAmount { get; set; }
    public int TenureMonths { get; set; }
    public decimal AnnualInterestRate { get; set; }  // percent, e.g. 12.5
    public int CreditScore { get; set; }             // CIBIL: 300-900

    // ----- Values derived by the engine -----
    public decimal ProposedEmi { get; set; }
    public decimal Foir { get; set; }                // ratio 0..n (0.45 = 45%)

    // ----- Decision + audit -----
    public DecisionStatus Decision { get; set; }
    public ApplicationStatus Status { get; set; }
    public List<string> DecisionReasons { get; set; } = new();

    // ----- Maker-checker -----
    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewComment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
