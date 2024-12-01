using SocialMediaApp.Persistencia.Servicios;
using SocialMediaApp.Persistencia.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
//llamar al metodo que inyecta nuestras dependencias
InyectarDependencias.ConfiguracionServicios(builder.Services);

builder.Configuration.AddJsonFile(@"C:\SocialMediaApp\SocialMediaApp.Persistencia\ConnectionStrings.json", optional: false, reloadOnChange: true);

//Configurar la conexion string
builder.Services.AddDbContext<SocialMediaDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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