using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavesService.Api.RequestModels;
using SavesService.Api.Services;
using SavesService.Core.Contracts;
using SavesService.Core.Exceptions;
using SavesService.Core.Models;

namespace SavesService.Api.Controllers;

[Authorize]
[ApiController]
public class GameSaveController : ControllerBase
{
    private readonly IGameService gameSaveService;

    public GameSaveController(IGameService gameSaveService)
    {
        this.gameSaveService = gameSaveService;
    }
    
    [HttpPost("api/saves")]
    public async Task<IActionResult> Save([FromForm] GameSaveRequest request)
    {
        try
        {
            var gameSave = Mappers.GameSaveReqToGameSave(request);
            var userClaim = User.Claims.FirstOrDefault(c=>c.Type=="UserId");
            if(userClaim == null)
                return BadRequest();
            
            gameSave.UserId = Guid.Parse(userClaim.Value);
            
            var stream = request.Save.OpenReadStream();
            
            await gameSaveService.CreateAsync(gameSave, stream, request.Save.FileName);
        }
        catch (Exception e)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("api/user/saves")]
    public async Task<IActionResult> GetUserSaves(int skip = 0, int take = 10)
    {
        List<GameSave> saves;
        
        try
        {
            var userClaim = User.Claims.FirstOrDefault(c=>c.Type=="UserId");
            if(userClaim == null)
                return BadRequest();
            
            var userId = Guid.Parse(userClaim.Value);
            
            saves = await gameSaveService.GetByUserIdAsync(userId, skip, take);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return Ok(saves);
    }

    [HttpGet("api/user/saves/{saveId:guid}")]
    public async Task<IActionResult> GetUserSave(Guid saveId)
    {
        GameSave save;
        try
        {
            save = await gameSaveService.GetByIdAsync(saveId);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        
        var mimetype = "application/octet-stream";
        
        return PhysicalFile(save.Path, mimetype, save.FileName);
    }
}