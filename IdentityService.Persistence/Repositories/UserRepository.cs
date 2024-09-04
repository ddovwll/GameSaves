using IdentityService.Core.Contracts;
using IdentityService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbHelper database;

    public UserRepository(DbHelper database)
    {
        this.database = database;
    }
    
    public async Task<User> AddAsync(User user)
    {
        await database.AddAsync(user);
        await database.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetByIdAsync(Guid userId)
    {
        var user = await database.Users.FindAsync(userId) ?? new User();
        return user;
    }

    public async Task<User> GetByNameAsync(string userName)
    {
        var user = await database.Users.FirstOrDefaultAsync(
            u => u.Name == userName) ?? new User();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        database.Update(user);
        await database.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(User user)
    {
        database.Remove(user);
        await database.SaveChangesAsync();
    }
}