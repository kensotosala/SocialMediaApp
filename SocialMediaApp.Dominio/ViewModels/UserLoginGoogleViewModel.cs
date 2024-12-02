using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.ViewModels
{
    public class UserLoginGoogleViewModel
    {
        public string NombreUsuario { get; set; }

        public string Email { get; set; }

        public string? Contraseña { get; set; }

        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? SalContraseña { get; set; }
        public string? FotoPerfil { get; set; }
        public string? Biografia { get; set; }
        public string? Ubicacion { get; set; }

        public string? Intereses { get; set; }

        public bool? EsPremium { get; set; }

        public DateTime? FechaRegistro { get; set; }
    }
}
