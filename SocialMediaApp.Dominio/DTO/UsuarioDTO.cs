using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.DTO
{
    public class UsuarioDTO
    {
        public int UsuarioId { get; set; }

        public string NombreUsuario { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Contraseña { get; set; } = null!;

        public string? FotoPerfil { get; set; }

        public string? Biografia { get; set; }

        public string? Ubicacion { get; set; }

        public string? Intereses { get; set; }

        public bool? EsPremium { get; set; }

        public DateTime? FechaRegistro { get; set; }

    }
}
