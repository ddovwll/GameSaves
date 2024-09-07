using SavesService.Application.Contracts;
using SavesService.Core.Contracts;
using SavesService.Core.Exceptions;
using SavesService.Core.Models;

namespace SavesService.Application.Services;

public class GameSaveService : IGameService
{
    private readonly IGameSaveRepository gameSaveRepository;
    private readonly IFileSystemService fileSystemService;

    public GameSaveService(IGameSaveRepository gameSaveRepository, IFileSystemService fileSystemService)
    {
        this.gameSaveRepository = gameSaveRepository;
        this.fileSystemService = fileSystemService;
    }

    public async Task<GameSave> CreateAsync(GameSave gameSave, Stream stream, string name)
    {
        gameSave.Id = Guid.NewGuid();
        gameSave.Date = DateTime.UtcNow;
        
        var pathName = await fileSystemService.SaveAsync(stream, gameSave.Path, name);
        gameSave.Path = pathName.Item1;
        gameSave.FileName = pathName.Item2;
        
        await gameSaveRepository.CreateAsync(gameSave);
        
        return gameSave;
    }

    public async Task<GameSave> GetByIdAsync(Guid gameSaveId)
    {
        var gameSave = await gameSaveRepository.GetByIdAsync(gameSaveId);

        if (gameSave.Id == Guid.Empty)
            throw new NotFoundException("Сохранение не найдено");
        
        return gameSave;
    }

    public async Task<List<GameSave>> GetByUserIdAsync(Guid userId, int skip, int take)
    {
        var gameSaveList = await gameSaveRepository.GetByUserIdAsync(userId, skip, take);

        if (gameSaveList.Count == 0)
            throw new NotFoundException("Сохранения не найдены");

        return gameSaveList;
    }

    public async Task<List<GameSave>> GetByGameAsync(string gameName, int skip, int take)
    {
        var gameSaveList = await gameSaveRepository.GetByGameAsync(gameName, skip, take);

        if (gameSaveList.Count == 0)
            throw new NotFoundException("Сохранения не найдены");

        return gameSaveList;
    }

    public async Task DeleteAsync(Guid gameSaveId)
    {
        var gameSaveFromDb = await gameSaveRepository.GetByIdAsync(gameSaveId);

        if (gameSaveFromDb.Id == Guid.Empty)
            throw new NotFoundException("Сохранение не найдено");
        
        fileSystemService.Delete(gameSaveFromDb.Path);
        
        await gameSaveRepository.DeleteAsync(gameSaveFromDb);
    }
}