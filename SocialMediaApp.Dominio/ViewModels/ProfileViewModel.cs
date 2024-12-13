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
        [Required(ErrorMessage = "Se quiere tu contraseña actual.")]
        public string password {  get; set; }

        [Required(ErrorMessage = "Se quiere una contraseña nueva.")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "La contraseña debe contener entre 8 and 16 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\!\@\#\$\%\^\&\*\(\)\,\.\?\"":\{\}\|<>\=\+])[A-Za-z\d\!\@\#\$\%\^\&\*\(\)\,\.\?\"":\{\}\|<>\=\+]+$",
        ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula y un carácter especial")]
        public string newPassword { get; set; }
        
        [Compare("newPassword", ErrorMessage = "Las contraseñas ingresadas no coinciden.")]
        [Required(ErrorMessage = "Se requiere confirmar la nueva contraseña.")]
        public string confirmPassword { get; set; }
    }
}
