namespace SavesService.Core.Models;

public class GameSave
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Game { get; set; }
    public string GameVersion { get; set; }
    public DateTime Date { get; set; }
    public bool Shared { get; set; }
}