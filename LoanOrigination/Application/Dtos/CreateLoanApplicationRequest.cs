namespace LoanOrigination.Application.Dtos;

/// <summary>What the caller posts to apply for a loan.</summary>
public class CreateLoanApplicationRequest
{
    public string ApplicantName { get; set; } = string.Empty;
    public int Age { get; set; }
    public decimal NetMonthlyIncome { get; set; }
    public decimal ExistingMonthlyEmi { get; set; }
    public decimal LoanAmount { get; set; }
    public int TenureMonths { get; set; }
    public decimal AnnualInterestRate { get; set; } = 12m;
    public int CreditScore { get; set; }
}
