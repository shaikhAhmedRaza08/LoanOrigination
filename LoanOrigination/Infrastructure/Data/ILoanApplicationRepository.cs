using LoanOrigination.Domain;

namespace LoanOrigination.Infrastructure.Data;

public interface ILoanApplicationRepository
{
    Task<LoanApplication> AddAsync(LoanApplication application);
    Task<LoanApplication?> GetByIdAsync(Guid id);
    Task<List<LoanApplication>> GetAllAsync();
    Task SaveChangesAsync();
}
