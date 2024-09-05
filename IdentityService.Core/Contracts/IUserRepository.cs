using IdentityService.Core.Models;

namespace IdentityService.Core.Contracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User> GetByIdAsync(Guid userId);
    Task<User> GetByNameAsync(string userName);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(User user);
}