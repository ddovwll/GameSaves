using IdentityService.Core.Models;

namespace IdentityService.Core.Contracts;

public interface ISessionService
{
    Task<Session> UpdateSessionAsync(Guid id, string fingerPrint);
    Task DeleteSessionAsync(Guid id, string fingerPrint);
    Task<Session> CreateSessionAsync(Guid userId, string fingerPrint);
}