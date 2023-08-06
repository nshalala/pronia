﻿using Pronia.ExtensionServices.Interfaces;
using Pronia.Helpers.Exceptions;
using Pronia.Helpers.Extensions;

namespace Pronia.ExtensionServices.Implements;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public void Delete(string? path)
    {
        if (String.IsNullOrEmpty(path) || String.IsNullOrWhiteSpace(path)) 
            throw new NullReferenceException();
        if(!path.StartsWith(_env.WebRootPath))
            path = Path.Combine(_env.WebRootPath, path);
        if(File.Exists(path))
            File.Delete(path);
    }

    public async Task SaveAsync(IFormFile file, string path)
    {
        FileStream fs = new FileStream(Path.Combine(_env.WebRootPath, path), FileMode.Create);
        await file.CopyToAsync(fs);
    }

    public async Task<string> UploadAsync(IFormFile file, string path, string contentType = "image", int mb = 2)
    {
        if (!file.isValidSize(mb)) throw new SizeLimitException("File size should not exceed " + mb);
        if (!file.isValidType(contentType)) throw new WrongTypeException("Wrong file type");
        string newFileName = _renameFile(file);
        _checkDirectory(path);
        path = Path.Combine(path, newFileName);
        await SaveAsync(file, path);
        return path;
    }
    private string _renameFile(IFormFile file)
        => Guid.NewGuid() + Path.GetExtension(file.FileName);
    private void _checkDirectory(string path)
    {
        if (!Directory.Exists(Path.Combine(_env.WebRootPath, path)))
        {
            Directory.CreateDirectory(Path.Combine(_env.WebRootPath, path));
        }
    }
}
