using SavesService.Api.RequestModels;
using SavesService.Core.Models;

namespace SavesService.Api.Services;

public static class Mappers
{
    public static GameSave GameSaveReqToGameSave(GameSaveRequest gameSaveRequest)
    {
        return new GameSave()
        {
            Name = gameSaveRequest.Name,
            Game = gameSaveRequest.Game,
            GameVersion = gameSaveRequest.GameVersion,
            Shared = gameSaveRequest.Shared
        };
    }
}