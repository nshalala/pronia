namespace Pronia.Helpers.Extensions;

public static class FileExtensions
{
    public static bool isValidSize(this IFormFile file, int size)
        => file.Length <= size * 1024 * 1024;
    public static bool isValidType(this IFormFile file, string contentType)
        => file.ContentType.StartsWith(contentType);
}
