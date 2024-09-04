using IdentityService.Application.Contracts;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace IdentityService.Infrastructure.Services;

public class PasswordEncrypt : IPasswordEncrypt
{
    public string Encrypt(string password, string salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(password,
            System.Text.Encoding.ASCII.GetBytes(salt),
            KeyDerivationPrf.HMACSHA512,
            5000,
            64));
    }
}