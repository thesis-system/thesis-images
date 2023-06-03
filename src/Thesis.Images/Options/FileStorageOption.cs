namespace Thesis.Images.Options;

/// <summary>
/// Опции сохранения файла
/// </summary>
public class FileStorageOption
{
    /// <summary>
    /// Путь к директории с изображениями
    /// </summary>
    public string Path { get; set; } = string.Empty;
    
    /// <summary>
    /// Путь к временной директории
    /// </summary>
    public string TempPath { get; set; } = string.Empty;
    
    /// <summary>
    /// Удалить временный файл после обработки через указанное количество секунд
    /// </summary>
    public int RemoveTempAt { get; set; }
}
