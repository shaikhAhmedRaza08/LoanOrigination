using LoanOrigination.Domain;

namespace LoanOrigination.Application.Rules;

/// <summary>
/// A single underwriting policy check. New policies are added by implementing this
/// interface and registering it in Program.cs - the engine needs no changes (Open/Closed).
/// </summary>
public interface IRule
{
    RuleResult Evaluate(LoanApplication application);
}
