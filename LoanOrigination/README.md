# Retail Loan Origination & Credit Assessment System (MVP)

A backend service that takes a retail loan application, runs an automated underwriting
engine, and returns a credit decision — **Approve**, **Refer**, or **Decline** — with the
reasons behind it. Referred cases go to a **maker-checker** review step.

Built with **ASP.NET Core 8 (Web API)**, **Entity Framework Core (SQLite)**, and **xUnit**.

---

## What it does (domain)

A Loan Origination System (LOS) moves a loan application from "submitted" to a decision.
This MVP models the credit-assessment core of that flow:

1. **Application intake** – applicant income, obligations, loan amount, tenure, CIBIL score.
2. **Derive financials** – the engine computes the **proposed EMI** (reducing-balance
   formula) and the **FOIR** (Fixed Obligation to Income Ratio).
3. **Underwriting rules** – each policy is a small rule returning Pass / Refer / Fail:
   - **Age** must be 21–60.
   - **CIBIL score**: ≥750 pass, 650–749 refer, <650 decline.
   - **FOIR** = (existing EMIs + proposed EMI) / net income: ≤50% pass, 50–60% refer, >60% decline.
   - **Minimum income** threshold.
4. **Decision** – aggregated conservatively: any Fail → **Decline**, else any Refer →
   **Refer**, else **Approve**.
5. **Maker-checker** – a reviewer approves/rejects any **Referred** application.

### Key terms
- **CIBIL score** – Indian credit score (300–900); higher = lower risk.
- **FOIR** – share of income already committed to debt; the core affordability check.
- **Maker-checker** – dual control: the engine "makes" a decision, a human "checks" it.

---

## Architecture

A single Web API project with clean, layered separation:

```
Domain/          Entities + enums (no dependencies)
Application/      Rules (IRule + 4 rules), the credit engine, orchestration service, DTOs
Infrastructure/   EF Core DbContext + repository
Api/Controllers/  Thin HTTP layer
```

- Controllers stay thin; business rules live in `Application`; data access is behind a
  repository interface — so the decision logic is unit-testable with no database or web host.
- Adding a new underwriting policy = add one `IRule` class + register it. The engine
  doesn't change (Open/Closed Principle).

---

## Run it

Requires the **.NET 8 SDK**.

```bash
# from the LoanOrigination folder
dotnet restore
dotnet run
```

Open **Swagger** at the HTTPS URL printed in the console (e.g. https://localhost:7xxx/swagger).
The SQLite database file `loanorigination.db` is created automatically on first run.

### Run the tests
```bash
# from the LoanOrigination.Tests folder
dotnet test
```

---

## API

| Method | Route | Purpose |
|--------|-------|---------|
| POST | `/api/LoanApplications` | Submit an application; returns the decision |
| GET  | `/api/LoanApplications/{id}` | Fetch one application |
| GET  | `/api/LoanApplications` | List all applications |
| POST | `/api/LoanApplications/{id}/review` | Maker-checker: approve/reject a referred case |

Sample requests are in `LoanOrigination.http` (use the VS Code REST Client or Visual Studio).

---

## Scoped out of this MVP (and how I'd add it next)
- Real credit-bureau integration (currently the score is an input).
- KYC / document upload and verification.
- Authentication & role-based access (maker vs checker identity).
- A React front end over these APIs.
- Disbursement, multi-product support, and EF Core migrations for schema versioning.
