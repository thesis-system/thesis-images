using Microsoft.Extensions.Options;
using Thesis.Images.Options;

namespace Thesis.Images.Repositories;

/// <summary>
/// Репозиторий изображений
/// </summary>
public class ImagesRepository
{
    private readonly string _fileStoragePath;

    /// <summary>
    /// Конструктор класса <see cref="ImagesRepository"/>
    /// </summary>
    /// <param name="fileStoragePath">Опции хранилища файлов</param>
    public ImagesRepository(IOptions<FileStorageOption> fileStoragePath)
    {
        _fileStoragePath = fileStoragePath.Value.Path;
    }
    
    /// <summary>
    /// Получить путь к изображению
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Полный путь до изображения</returns>
    public Task<string?> GetFileName(Guid id)
    {
        var path = Path.Combine(_fileStoragePath, $"{id}.jpg");
        return Task.FromResult(File.Exists(path) ? path : null);
    }

    /// <summary>
    /// Сохранить изображение
    /// </summary>
    /// <param name="file">Файл</param>
    /// <returns>Идентификатор изображения</returns>
    public async Task<Guid> SaveFile(IFormFile file)
    {
        var id = Guid.NewGuid();
        var path = Path.Combine(_fileStoragePath, $"{id}.jpg");
        await using var writer = File.Create(path);
        await file.CopyToAsync(writer);
        return id;
    }
}