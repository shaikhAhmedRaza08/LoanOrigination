using LoanOrigination.Application.Rules;
using LoanOrigination.Application.Services;
using LoanOrigination.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Framework services ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Persistence (EF Core + SQLite) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Default")
        ?? "Data Source=loanorigination.db"));

// --- Underwriting rules (registered as a collection; the engine runs all of them) ---
builder.Services.AddScoped<IRule, AgeRule>();
builder.Services.AddScoped<IRule, CreditScoreRule>();
builder.Services.AddScoped<IRule, FoirRule>();
builder.Services.AddScoped<IRule, MinimumIncomeRule>();

// --- Application services ---
builder.Services.AddScoped<ICreditAssessmentService, CreditAssessmentService>();
builder.Services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
builder.Services.AddScoped<ILoanApplicationService, LoanApplicationService>();

var app = builder.Build();

// MVP convenience: create the database on startup.
// In production you would use EF Core migrations instead.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
