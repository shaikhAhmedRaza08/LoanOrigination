using LoanOrigination.Domain;
using Microsoft.EntityFrameworkCore;

namespace LoanOrigination.Infrastructure.Data;

public class LoanApplicationRepository : ILoanApplicationRepository
{
    private readonly AppDbContext _db;

    public LoanApplicationRepository(AppDbContext db) => _db = db;

    public async Task<LoanApplication> AddAsync(LoanApplication application)
    {
        await _db.LoanApplications.AddAsync(application);
        await _db.SaveChangesAsync();
        return application;
    }

    public async Task<LoanApplication?> GetByIdAsync(Guid id) =>
        await _db.LoanApplications.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<LoanApplication>> GetAllAsync() =>
        await _db.LoanApplications.OrderByDescending(x => x.CreatedAt).ToListAsync();

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
