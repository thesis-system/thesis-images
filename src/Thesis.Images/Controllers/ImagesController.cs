using Microsoft.AspNetCore.Mvc;
using Thesis.Images.Repositories;

namespace Thesis.Images.Controllers;

/// <summary>
/// Контроллер для работы с изображениями
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly ImagesRepository _imagesRepository;
    private readonly ILogger<ImagesController> _logger;

    /// <summary>
    /// Конструктор класса <see cref="ImagesController"/>
    /// </summary>
    /// <param name="imagesRepository">Репозиторий изображений</param>
    /// <param name="logger">Логгер</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ImagesController(ImagesRepository imagesRepository, ILogger<ImagesController> logger)
    {
        _imagesRepository = imagesRepository ?? throw new ArgumentNullException(nameof(imagesRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    /// <summary>
    /// Получает картинку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <response code="200">Изображение</response>
    /// <response code="400">Некорректный идентификатор</response>
    /// <response code="404">Изображение не найдено</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PhysicalFileResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Get(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(nameof(id));

        var filePath = await _imagesRepository.GetFileName(id);
        return filePath != null 
            ? PhysicalFile(filePath, "image/jpeg")
            : NotFound(id);
    }
    
    /// <summary>
    /// Загружает картинку
    /// </summary>
    /// <param name="image">Идентификатор добавленного изображения</param>
    /// <response code="200">Изображение добавлено</response>
    /// <response code="400">Некорректный тип изображения. Ожидается image/jpeg.</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> Post(IFormFile image)
    {
        if (image.ContentType != "image/jpeg")
            return BadRequest(nameof(image.ContentType));

        var fileId = await _imagesRepository.SaveFile(image);
        return Ok(fileId);
    }
}
