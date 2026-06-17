# Quickstart

Requires the .NET 8 SDK (`dotnet --version` should print 8.x).

## Run the API
```bash
cd LoanOrigination
dotnet restore
dotnet run
```
A browser opens at http://localhost:5000/swagger. Submit an application from there.
The SQLite database (loanorigination.db) is created automatically.

## Run the tests
```bash
cd LoanOrigination.Tests
dotnet test
```

## (Optional) Create a solution file that ties both projects together
```bash
# from the RetailLoanOrigination folder
dotnet new sln -n RetailLoanOrigination
dotnet sln add LoanOrigination/LoanOrigination.csproj
dotnet sln add LoanOrigination.Tests/LoanOrigination.Tests.csproj
```

## Put it on GitHub (do this in several commits, not one)
```bash
cd RetailLoanOrigination
git init
git add .
git commit -m "Domain model and enums"
# ...keep committing as you read through each layer...
```
See README.md inside LoanOrigination/ for the full domain explanation and API reference.
