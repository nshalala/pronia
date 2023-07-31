namespace Pronia.ExtensionServices.Interfaces;

public interface IFileService
{
    Task<string> UploadAsync(IFormFile file, string path, string contentType, int mb);
    Task SaveAsync(IFormFile file);
    void Delete(string path);
}
