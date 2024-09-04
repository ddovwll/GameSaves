namespace IdentityService.Application.Contracts;

public interface IPasswordEncrypt
{
    public string Encrypt(string password, string salt);
}