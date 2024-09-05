namespace IdentityService.Application.Contracts;

public interface IJwtCreator
{
    string CreateToken(Guid userId);
}