using B1WPFTask.Models;
using Microsoft.EntityFrameworkCore;

namespace B1WPFTask.Data;
internal sealed class RandomRowDBContext : DbContext
{
    public RandomRowDBContext(DbContextOptions<RandomRowDBContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<RandomRow> RandomRows { get; set; }
}