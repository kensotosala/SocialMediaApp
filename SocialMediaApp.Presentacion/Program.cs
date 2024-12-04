using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Persistencia.Data;
using SocialMediaApp.Persistencia.Servicios;
using SocialMediaApp.Presentacion.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Llamar al m�todo que inyecta nuestras dependencias
InyectarDependencias.ConfiguracionServicios(builder.Services);


// Add services permite consumir el API.
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllersWithViews();


//Contfigurar la conexion string
builder.Services.AddDbContext<SocialMediaDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SocialMediaDB"))
    );

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



// This is a test


