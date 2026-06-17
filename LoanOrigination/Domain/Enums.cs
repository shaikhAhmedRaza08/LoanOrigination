namespace LoanOrigination.Domain;

/// <summary>The auto-decision produced by the credit engine.</summary>
public enum DecisionStatus
{
    Approve,
    Refer,
    Decline
}

/// <summary>Where the application currently sits in the workflow.</summary>
public enum ApplicationStatus
{
    AutoApproved,
    Referred,        // waiting for a human checker
    AutoDeclined,
    ReviewApproved,  // checker approved a referred case
    ReviewRejected   // checker rejected a referred case
}

/// <summary>The result a single underwriting rule can return.</summary>
public enum RuleOutcome
{
    Pass,
    Refer,
    Fail
}
