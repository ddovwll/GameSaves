using IdentityService.Core.Contracts;
using IdentityService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Persistence.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly DbHelper database;

    public SessionRepository(DbHelper database)
    {
        this.database = database;
    }
    
    public async Task<Session> AddAsync(Session session)
    {
        await database.AddAsync(session);
        await database.SaveChangesAsync();
        return session;
    }

    public async Task<Session> GetAsync(Guid id, string fingerprint)
    {
        var session = await database.Sessions.FirstOrDefaultAsync(
            s => s.Id == id && s.FingerPrint == fingerprint) ?? new Session();
        return session;
    }

    public async Task DeleteAsync(Session session)
    {
        database.Sessions.Remove(session);
        await database.SaveChangesAsync();
    }
}