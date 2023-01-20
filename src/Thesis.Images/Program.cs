using Thesis.Images.Options;
using Thesis.Images.Repositories;
using Thesis.Services.Common.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var fileStorageSection = builder.Configuration.GetSection(nameof(FileStorageOption));
builder.Services.AddOptions<FileStorageOption>()
    .Bind(fileStorageSection);

builder.Services.AddScoped<ImagesRepository>();

var app = builder.BuildWebApplication();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();