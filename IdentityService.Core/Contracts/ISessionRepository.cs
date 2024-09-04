using IdentityService.Core.Models;

namespace IdentityService.Core.Contracts;

public interface ISessionRepository
{
    Task<Session> AddAsync(Session session);
    Task<Session> GetAsync(Guid id, string fingerprint);
    Task DeleteAsync(Session session);
}