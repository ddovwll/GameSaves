namespace SavesService.Api.RequestModels;

public class GameSaveRequest
{
    public string Name { get; init; }
    public string Game { get; init; }
    public string GameVersion { get; init; }
    public bool Shared { get; init; }
    public IFormFile Save { get; init; }
}