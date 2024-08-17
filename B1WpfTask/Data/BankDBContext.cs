using B1WPFTask.Models;
using Microsoft.EntityFrameworkCore;

namespace B1WPFTask.Data;

internal sealed class BankDBContext : DbContext
{
    public BankDBContext(DbContextOptions<BankDBContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<InputFile> InputFiles { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<BankAccountClass> BankAccountClasses { get; set; }
    public DbSet<InputBalance> InputBalances { get; set; }
    public DbSet<OutputBalance> OutputBalances { get; set; }
    public DbSet<Turnover> Turnovers { get; set; }
}