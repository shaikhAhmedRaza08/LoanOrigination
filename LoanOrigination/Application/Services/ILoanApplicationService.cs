using LoanOrigination.Application.Dtos;

namespace LoanOrigination.Application.Services;

public interface ILoanApplicationService
{
    Task<LoanApplicationResponse> SubmitAsync(CreateLoanApplicationRequest request);
    Task<LoanApplicationResponse?> GetAsync(Guid id);
    Task<List<LoanApplicationResponse>> GetAllAsync();
    Task<(bool ok, string? error, LoanApplicationResponse? result)> ReviewAsync(Guid id, ReviewRequest request);
}
