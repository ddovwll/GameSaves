using IdentityService.Api.RequestModels;
using IdentityService.Core.Models;

namespace IdentityService.Api.Services;

public static class Mapper
{
    public static User UserRegisterToUser(UserLogInOn userLogInOn)
    {
        return new User()
        {
            Name = userLogInOn.Name,
            Password = userLogInOn.Password
        };
    }
}