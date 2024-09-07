using Microsoft.EntityFrameworkCore;
using SavesService.Core.Models;

namespace SavesService.Persistence;

public class DbHelper : DbContext
{
    public DbHelper(DbContextOptions<DbHelper> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<GameSave> Saves { get; set; }
}