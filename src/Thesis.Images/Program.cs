using Thesis.Images.Options;
using Thesis.Images.Repositories;
using Thesis.Services.Common.Helpers;

var builder = WebApplication.CreateBuilder(args);

var fileStorageSection = builder.Configuration.GetSection(nameof(FileStorageOption));
builder.Services.AddOptions<FileStorageOption>()
    .Bind(fileStorageSection);

builder.Services.AddScoped<ImagesRepository>();

var app = builder.BuildWebApplication();

app.Run();