using ImageMagick;
using Microsoft.Extensions.Options;
using Thesis.Images.Options;

namespace Thesis.Images.Repositories;

/// <summary>
/// Репозиторий изображений
/// </summary>
public class ImagesRepository
{
    private readonly FileStorageOption _fileStorageOption;

    /// <summary>
    /// Конструктор класса <see cref="ImagesRepository"/>
    /// </summary>
    /// <param name="fileStoragePath">Опции хранилища файлов</param>
    public ImagesRepository(IOptions<FileStorageOption> fileStoragePath)
    {
        _fileStorageOption = fileStoragePath.Value;
    }

    /// <summary>
    /// Получить путь к изображению
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Полный путь до изображения</returns>
    public Task<string?> GetFileName(Guid id)
    {
        var path = Path.Combine(_fileStorageOption.Path, $"{id}.jpg");
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
        var path = Path.Combine(_fileStorageOption.Path, $"{id}.jpg");
        await using var writer = File.Create(path);
        await file.CopyToAsync(writer);
        return id;
    }
    
    /// <summary>
    /// Удалить изображение
    /// </summary>
    /// <param name="path">Путь к изображению</param>
    public static void RemoveFile(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }
    
    /// <summary>
    /// Удалить временный файл по истечению времени
    /// </summary>
    /// <param name="path">Путь к изображению</param>
    public void RemoveTemp(string path)
    {
        Task.Run(async () =>
        {
            await Task.Delay(_fileStorageOption.RemoveTempAt);
            RemoveFile(path);
        });
    }
    
    /// <summary>
    /// Обработать изображение
    /// </summary>
    /// <param name="filePath">Путь к изображению</param>
    /// <param name="width">Ширина</param>
    /// <param name="height">Высота</param>
    /// <param name="quality">Качество</param>
    /// <returns></returns>
    public async Task<string> ProcessImage(string filePath, int? width, int? height, int? quality)
    {
        using var image = new MagickImage(filePath);
        image.Resize(width ?? image.Width, height ?? image.Height);
        image.Quality = quality ?? image.Quality;

        var tempName = Path.Combine(_fileStorageOption.TempPath, $"{Guid.NewGuid()}.jpg");
        await image.WriteAsync(tempName);
        return tempName;
    }
}