using LoanOrigination.Domain;

namespace LoanOrigination.Application.Rules;

/// <summary>The outcome of evaluating one rule, with a human-readable reason for auditability.</summary>
public record RuleResult(string RuleName, RuleOutcome Outcome, string Reason);
