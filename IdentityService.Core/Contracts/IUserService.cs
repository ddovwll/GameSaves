using IdentityService.Core.Models;

namespace IdentityService.Core.Contracts;

public interface IUserService
{
    Task<User> AddAsync(User user);
    Task<User> GetByIdAsync(Guid userId);
    Task<User> GetByNameAsync(string userName);
    Task<User> UpdateNameAsync(Guid userId, string userName);
    Task<User> UpdatePasswordAsync(Guid userId, string password);
    Task DeleteAsync(Guid userId);
    Task<bool> AuthenticateAsync(string username, string password);
}