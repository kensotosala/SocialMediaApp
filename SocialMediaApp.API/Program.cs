using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Persistencia.Data;
using SocialMediaApp.Persistencia.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Llamar al método que inyecta nuestras dependencias
InyectarDependencias.ConfiguracionServicios(builder.Services);

// Configurar la conexión a la base de datos (string)
builder.Services.AddDbContext<SocialMediaDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SocialMediaApp"))
);

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5048")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();