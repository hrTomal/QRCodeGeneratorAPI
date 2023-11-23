using Interface.IService;
using Service.DynamicImageService;
using Service.ImageUploadService;
using Service.QRService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IDynamicImageService, DynamicImageService>();
builder.Services.AddScoped<IQRService, QRService>();
builder.Services.AddScoped<IImageUploadService, ImageUploadService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
