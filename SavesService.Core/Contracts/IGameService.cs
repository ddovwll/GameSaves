using SavesService.Core.Models;

namespace SavesService.Core.Contracts;

public interface IGameService
{
    Task<GameSave> CreateAsync(GameSave gameSave, Stream stream, string name);
    Task<GameSave> GetByIdAsync(Guid gameSaveId);
    Task<List<GameSave>> GetByUserIdAsync(Guid userId, int skip, int take);
    Task<List<GameSave>> GetByGameAsync(string gameName, int skip, int take);
    Task DeleteAsync(Guid gameSaveId);
}