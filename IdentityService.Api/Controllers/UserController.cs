using IdentityService.Api.RequestModels;
using IdentityService.Api.Services;
using IdentityService.Application.Contracts;
using IdentityService.Core.Contracts;
using IdentityService.Core.Exceptions;
using IdentityService.Core.Models;
using IdentityService.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ISessionService sessionService;
    private readonly IJwtCreator jwtCreator;

    public UserController(IUserService userService, ISessionService sessionService, IJwtCreator jwtCreator)
    {
        this.userService = userService;
        this.sessionService = sessionService;
        this.jwtCreator = jwtCreator;
    }

    [HttpPost("api/user/register")]
    public async Task<IActionResult> Register([FromBody] UserLogInOn userLogInOn)
    {
        var user = Mapper.UserRegisterToUser(userLogInOn);

        try
        {
            await userService.AddAsync(user);
        }
        catch (NotValidException e)
        {
            return BadRequest(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }

        return Ok();
    }

    [HttpPost("api/user/login")]
    public async Task<IActionResult> Login([FromBody] UserLogInOn userLogInOn)
    {
        User user;
        bool authResult;
        var fingerprint = Request.Headers["FingerPrint"];

        try
        {
            authResult = await userService
                .AuthenticateAsync(userLogInOn.Name, userLogInOn.Password);
            
            user = await userService.GetByNameAsync(userLogInOn.Name);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        
        if (!authResult)
            return Unauthorized("Неверное имя пользователя или пароль");

        if (string.IsNullOrEmpty(fingerprint.ToString()))
            return BadRequest();
        
        var session = await sessionService.CreateSessionAsync(user.Id, fingerprint.ToString());
        var jwt = jwtCreator.CreateToken(user.Id);
        
        Response.Cookies.Append("refreshToken", session.Id.ToString(), new CookieOptions { HttpOnly = true });

        return Ok(jwt);
    }
    
    [HttpPost("api/user/refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshModel refreshModel)
    {
        if(!Guid.TryParse(Request.Cookies["refreshToken"], out var refreshToken))
            return BadRequest();
        
        Session session;

        try
        {
            session = await sessionService.UpdateSessionAsync(refreshToken, refreshModel.FingerPrint);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
        
        var jwt = jwtCreator.CreateToken(session.UserId);
        
        Response.Cookies.Append("refreshToken", session.Id.ToString(), new CookieOptions { HttpOnly = true });

        return Ok(jwt);
    }
}