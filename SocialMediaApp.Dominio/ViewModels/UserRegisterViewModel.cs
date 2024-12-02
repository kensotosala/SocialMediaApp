using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Dominio.ViewModels
{
    public class UserRegisterViewModel
    {

        [Required(ErrorMessage = "Se requiere un nombre de usuario.")]
        [MinLength(6, ErrorMessage = "El nombre de usuario debe tener 6 o más caracteres.")]
        public string NombreUsuario { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere un correo electrónico.")]
        [EmailAddress(ErrorMessage = "Correo electrónico con formato incorrecto.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Se quiere una contraseña.")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "La contraseña debe contener entre 8 and 16 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\!\@\#\$\%\^\&\*\(\)\,\.\?\"":\{\}\|<>\=\+])[A-Za-z\d\!\@\#\$\%\^\&\*\(\)\,\.\?\"":\{\}\|<>\=\+]+$",
                ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula y un carácter especial")]
        public string Contraseña { get; set; } = null!;

        [Compare("Contraseña", ErrorMessage = "Las contraseñas ingresadas no coinciden.")]
        [Required(ErrorMessage = "Se debe confirmar la contraseña.")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "La contraseña debe contener entre 8 and 16 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\!\@\#\$\%\^\&\*\(\)\,\.\?\"":\{\}\|<>\=\+])[A-Za-z\d\!\@\#\$\%\^\&\*\(\)\,\.\?\"":\{\}\|<>\=\+]+$",
                ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula y un carácter especial")]
        public string ConfirmarContraseña { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere un nombre.")]
        [MinLength(2, ErrorMessage = "El nombre debe tener 2 o más caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere un apellido.")]
        [MinLength(2, ErrorMessage = "El apellido debe tener 2 o más caracteres.")]
        public string Apellido {get; set; } = null!;
        public string? SalContraseña { get; set; }
        public string? FotoPerfil { get; set; }
        public string? Biografia { get; set; }
        public string? Ubicacion { get; set; }

        public string? Intereses { get; set; }

        public bool? EsPremium { get; set; }

        public DateTime? FechaRegistro { get; set; }
    }
}
