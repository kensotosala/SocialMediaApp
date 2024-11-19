using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.ViewModels
{
    public class UserRegisterViewModel
    {

        [Required(ErrorMessage = "Se requiere un nombre de usuario.")]
        [MinLength(6, ErrorMessage = "El nombre de usuario debe tener 6 o más caracteres.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Se requiere un correo electrónico.")]
        [EmailAddress(ErrorMessage = "Correo electrónico con formato incorrecto.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Se quiere una contraseña.")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "La contraseña debe contener entre 8 and 16 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\!\@\#\$\%\^\&\*\(\)\,\.\?\"":\{\}\|<>])[A-Za-z\d\!\@\#\$\%\^\&\*\(\)\,\.\?\"":\{\}\|<>]+$",
                ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula y un carácter especial")]

        public string Contraseña { get; set; }
        public string? FotoPerfil { get; set; }
        public string? Biografia { get; set; }
        public string? Ubicacion { get; set; }

        public string? Intereses { get; set; }

        public bool? EsPremium { get; set; }

        public DateTime? FechaRegistro { get; set; }
    }
}
