using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.ViewModels
{
    public class ProfileViewModel
    {
        public string? NombreUsuario { get; set; }

        [Required(ErrorMessage = "Se requiere un nombre.")]
        [MinLength(2, ErrorMessage = "El nombre debe tener 2 o más caracteres.")]
        public string Nombre { get; set; } = null!;


        [Required(ErrorMessage = "Se requiere un apellido.")]
        [MinLength(2, ErrorMessage = "El apellido debe tener 2 o más caracteres.")]
        public string Apellido { get; set; } = null!;
        
        
        public string? Biografia{ get; set; }

        
        [MinLength(2, ErrorMessage = "Tu ubicacion debe tener al menos 2 caracteres.")]
        public string? Ubicacion { get; set; }

        
        [Required(ErrorMessage = "Se requiere un correo electrónico.")]
        [EmailAddress(ErrorMessage = "Correo electrónico con formato incorrecto.")]
        public string Email { get; set; } = null!;

    }
}
