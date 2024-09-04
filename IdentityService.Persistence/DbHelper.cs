using IdentityService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Persistence;

public class DbHelper : DbContext
{
    public DbHelper(DbContextOptions<DbHelper> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
}