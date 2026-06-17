using LoanOrigination.Domain;

namespace LoanOrigination.Application.Services;

/// <summary>Computes EMI/FOIR, runs every rule, and writes the decision onto the application.</summary>
public interface ICreditAssessmentService
{
    void Assess(LoanApplication application);
}
