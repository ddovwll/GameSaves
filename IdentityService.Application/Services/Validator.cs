using IdentityService.Core.Models;

namespace IdentityService.Application.Services;

public static class Validator
{
    public static bool ValidateUser(User user)
    {
        if (user.Id == Guid.Empty
            || string.IsNullOrWhiteSpace(user.Name)
            || string.IsNullOrWhiteSpace(user.Password))
            return false;
        
        return true;
    }
}