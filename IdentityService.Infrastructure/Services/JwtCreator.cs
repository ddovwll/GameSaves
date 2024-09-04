using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Application.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Infrastructure.Services;

public class JwtCreator : IJwtCreator
{
    private readonly string securityKeyString;

    public JwtCreator(string securityKeyString)
    {
        this.securityKeyString = securityKeyString;
    }
    
    public string CreateToken(Guid userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKeyString));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("UserId", userId.ToString()),
        };

        var token = new JwtSecurityToken(
            "GameSavesIdentityService",
            "GameSavesServices",
            expires: DateTime.Now.AddHours(0.5),
            signingCredentials: credentials,
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}