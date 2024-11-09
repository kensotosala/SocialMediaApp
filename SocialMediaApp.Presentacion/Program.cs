using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Persistencia.Data;
using SocialMediaApp.Persistencia.Servicios;

var builder = WebApplication.CreateBuilder(args);

//llamar al metodo que inyecta nuestras dependencias
InyectarDependencias.ConfiguracionServicios(builder.Services);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
//Configurar la conexion string
builder.Services.AddDbContext<SocialMediaDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SocialMediaApp"))
    );



var app = builder.Build();

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


