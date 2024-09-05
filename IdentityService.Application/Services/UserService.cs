using IdentityService.Application.Contracts;
using IdentityService.Core.Contracts;
using IdentityService.Core.Exceptions;
using IdentityService.Core.Models;

namespace IdentityService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordEncrypt passwordEncryptor;

    public UserService(IUserRepository userRepository, 
        IPasswordEncrypt passwordEncryptor)
    {
        this.userRepository = userRepository;
        this.passwordEncryptor = passwordEncryptor;
    }
    
    public async Task<User> AddAsync(User user)
    {
        user.Id = Guid.NewGuid();
        if (!Validator.ValidateUser(user))
            throw new NotValidException("Некорректные пользовательские данные");

        if ((await userRepository.GetByNameAsync(user.Name)).Id != Guid.Empty)
            throw new ConflictException("Пользователь с данным именем существует");
        
        user.Salt = Guid.NewGuid().ToString();
        user.Password = passwordEncryptor.Encrypt(user.Password, user.Salt);

        await userRepository.AddAsync(user);
        
        return user;
    }

    public async Task<User> GetByIdAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);

        if (user.Id == Guid.Empty)
            throw new NotFoundException("Пользователь не найден");
        
        return user;
    }

    public async Task<User> GetByNameAsync(string userName)
    {
        var user = await userRepository.GetByNameAsync(userName);

        if (user.Id == Guid.Empty)
            throw new NotFoundException("Пользователь с данным именем не найден");
        
        return user;
    }

    public async Task<User> UpdateNameAsync(Guid id, string userName)
    {
        if (string.IsNullOrEmpty(userName))
            throw new NotValidException("Некорректное имя пользователя");
        
        var user = await userRepository.GetByIdAsync(id);

        if (user.Id == Guid.Empty)
            throw new NotFoundException("Пользователь не найден");
        
        user.Name = userName;
        
        await userRepository.UpdateAsync(user);
        
        return user;
    }

    public async Task<User> UpdatePasswordAsync(Guid id, string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new NotValidException("Некорректный пароль");
        
        var user = await userRepository.GetByIdAsync(id);

        if (user.Id == Guid.Empty)
            throw new NotFoundException("Пользователь не найден");

        user.Salt = Guid.NewGuid().ToString();
        user.Password = passwordEncryptor.Encrypt(password, user.Salt);
        
        await userRepository.UpdateAsync(user);
        
        return user;
    }

    public async Task DeleteAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        
        if (user.Id == Guid.Empty)
            throw new NotFoundException("Пользователь с данным именем не найден");
        
        await userRepository.DeleteAsync(user);
    }

    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        var user = await userRepository.GetByNameAsync(username);

        if (user.Id == Guid.Empty)
            throw new NotFoundException("Пользователь не найден");

        if (user.Password != passwordEncryptor.Encrypt(password, user.Salt))
            return false;

        return true;
    }
}