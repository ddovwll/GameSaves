using SavesService.Application.Contracts;

namespace SavesService.Infrastructure.Services;

public class FileSystemService : IFileSystemService
{
    public async Task<(string, string)> SaveAsync(Stream file, string path, string name)
    {
        if (file == null || file.Length == 0)
            throw new Exception();

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var fileName = $"{Guid.NewGuid()}_{name}";
        var filePath = Path.Combine(uploadPath, fileName);
        
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return (filePath, fileName);
    }

    public void Delete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        
        else throw new FileNotFoundException();
    }
}