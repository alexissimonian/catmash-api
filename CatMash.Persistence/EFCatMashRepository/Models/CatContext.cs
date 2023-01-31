using Microsoft.EntityFrameworkCore;

namespace CatMash.Persistence.EFCatMashRepository.Models;

public class CatContext: DbContext
{
    public CatContext(DbContextOptions<CatContext> options)
        : base(options)
    {
    }

    public DbSet<CatScore> CatScores { get; set; } = null!; 
}