using IdentityService.Core.Contracts;
using IdentityService.Core.Exceptions;
using IdentityService.Core.Models;

namespace IdentityService.Application.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository sessionRepository;

    public SessionService(ISessionRepository sessionRepository)
    {
        this.sessionRepository = sessionRepository;
    }
    
    public async Task<Session> UpdateSessionAsync(Guid id, string fingerPrint)
    {
        var sessionFromDb = await sessionRepository.GetAsync(id, fingerPrint);

        if (sessionFromDb.Id == Guid.Empty)
            throw new NotFoundException("Сессия не найдена");

        if (DateTime.Now > sessionFromDb.CreatedOn.AddDays(Session.ExpiresOnDays))
        {
            await sessionRepository.DeleteAsync(sessionFromDb);
            throw new OldSessionException("Сессия устарела");
        }
        
        await sessionRepository.DeleteAsync(sessionFromDb);
        sessionFromDb.Id = Guid.NewGuid();
        await sessionRepository.AddAsync(sessionFromDb);
        
        return sessionFromDb;
    }

    public async Task DeleteSessionAsync(Guid id, string fingerPrint)
    {
        var session = await sessionRepository.GetAsync(id, fingerPrint);
        
        if(session.Id == Guid.Empty)
            throw new KeyNotFoundException("Сессия не найдена");
        
        await sessionRepository.DeleteAsync(session);
    }

    public async Task<Session> CreateSessionAsync(Guid userId, string fingerPrint)
    {
        var sessionFromDb = await sessionRepository.GetAsync(userId, fingerPrint);

        if (sessionFromDb.Id != Guid.Empty)
        {
            await sessionRepository.DeleteAsync(sessionFromDb);
            
            sessionFromDb.Id = Guid.NewGuid();
            sessionFromDb.CreatedOn = DateTime.Now;
            
            await sessionRepository.AddAsync(sessionFromDb);
            
            return sessionFromDb;
        }

        var session = new Session()
        {
            Id = Guid.NewGuid(),
            CreatedOn = DateTime.Now,
            FingerPrint = fingerPrint,
            UserId = userId
        };
        
        await sessionRepository.AddAsync(session);
        
        return session;
    }
}