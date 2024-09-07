namespace SavesService.Application.Contracts;

public interface IFileSystemService
{
    Task<(string, string)> SaveAsync(Stream file, string path, string name);
    void Delete(string path);
}