namespace LoanOrigination.Application.Dtos;

/// <summary>A checker's decision on a referred application (maker-checker).</summary>
public class ReviewRequest
{
    public bool Approve { get; set; }
    public string ReviewedBy { get; set; } = string.Empty;
    public string? Comment { get; set; }
}
