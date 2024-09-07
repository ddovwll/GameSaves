using Microsoft.EntityFrameworkCore;
using SavesService.Core.Contracts;
using SavesService.Core.Models;

namespace SavesService.Persistence.Repositories;

public class GameSaveRepository : IGameSaveRepository
{
    private readonly DbHelper database;

    public GameSaveRepository(DbHelper database)
    {
        this.database = database;
    }
    
    public async Task<GameSave> CreateAsync(GameSave gameSave)
    {
        await database.Saves.AddAsync(gameSave);
        await database.SaveChangesAsync();
        return gameSave;
    }

    public async Task<GameSave> GetByIdAsync(Guid gameSaveId)
    {
        var gameSave = await database.Saves.FindAsync(gameSaveId) ?? new GameSave();
        return gameSave;
    }

    public async Task<List<GameSave>> GetByUserIdAsync(Guid userId, int skip, int take)
    {
        var saves = await database.Saves
            .Where(s => s.UserId == userId)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return saves;
    }

    public async Task<List<GameSave>> GetByGameAsync(string gameName, int skip, int take)
    {
        var saves = await database.Saves
            .Where(s => s.Game.ToLower() == gameName.ToLower())
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return saves;
    }

    public async Task DeleteAsync(GameSave gameSave)
    {
        database.Remove(gameSave);
        await database.SaveChangesAsync();
    }
}