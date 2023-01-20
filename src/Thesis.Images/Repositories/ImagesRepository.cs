using System.Web;
using Microsoft.Extensions.Options;
using Thesis.Images.Options;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostEnvironment;

namespace Thesis.Images.Repositories;

/// <summary>
/// Репозиторий изображений
/// </summary>
public class ImagesRepository
{
    private readonly string _fileStoragePath;
    private readonly IHostEnvironment _hostEnvironment;

    /// <summary>
    /// Конструктор класса <see cref="ImagesRepository"/>
    /// </summary>
    /// <param name="fileStoragePath">Опции хранилища файлов</param>
    /// <param name="hostEnvironment">Опции окружения хоста</param>
    public ImagesRepository(IOptions<FileStorageOption> fileStoragePath, IHostEnvironment hostEnvironment)
    {
        _fileStoragePath = fileStoragePath.Value.Path;
        _hostEnvironment = hostEnvironment;
    }
    
    /// <summary>
    /// Получить путь к изображению
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Полный путь до изображения</returns>
    public Task<string?> GetFileName(Guid id)
    {
        var path = Path.Combine(_hostEnvironment.ContentRootPath, Path.Combine(_fileStoragePath, $"{id}.jpg"));
        return Task.FromResult(File.Exists(path) ? path : null);
    }

    /// <summary>
    /// Сохранить изображение
    /// </summary>
    /// <param name="file">Файл</param>
    /// <returns>Идентификатор изображения</returns>
    public async Task<Guid> SaveFile(IFormFile file)
    {
        if (!Directory.Exists(_fileStoragePath))
            Directory.CreateDirectory(_fileStoragePath);
        
        var id = Guid.NewGuid();
        var path = Path.Combine(_hostEnvironment.ContentRootPath, Path.Combine(_fileStoragePath, $"{id}.jpg"));
        await using var writer = File.Create(path);
        await file.CopyToAsync(writer);
        return id;
    }
}