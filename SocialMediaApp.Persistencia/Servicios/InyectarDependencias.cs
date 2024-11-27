using Microsoft.Extensions.DependencyInjection;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Persistencia.Servicios
{
    public class InyectarDependencias
    {
        public static void ConfiguracionServicios(IServiceCollection servicios)
        {
            servicios.AddScoped<IEvento,RepositorioEvento>();
            servicios.AddScoped<INotificaciones, RepositorioNotificaciones>();
            servicios.AddScoped<IBuscar_Usuario, RepositorioBuscar_Usuario>();

        }

    }
}
