using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Se requiere tu contraseña actual.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Se requiere una contraseña nueva.")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "La contraseña debe contener entre 8 and 16 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\!\@\#\$\%\^\&\*\(\)\,\.\?\"":\{\}\|<>\=\+])[A-Za-z\d\!\@\#\$\%\^\&\*\(\)\,\.\?\"":\{\}\|<>\=\+]+$",
        ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula y un carácter especial")]
        public string newPassword { get; set; }

        [Compare("newPassword", ErrorMessage = "Las contraseñas ingresadas no coinciden.")]
        [Required(ErrorMessage = "Se requiere confirmar la nueva contraseña.")]
        public string confirmPassword { get; set; }
    }

    public class ChangeQuestionViewModel
    {
        [Required(ErrorMessage = "Se requiere una pregunta de seguridad,.")]
        public string Pregunta { get; set; }

        [Required(ErrorMessage = "Se requiere una respuesta.")]
        public string Respuesta { get; set; }
    }

    public class ChangeSecurityViewModel
    {
        public ChangePasswordViewModel PasswordChange { get; set; }
        public ChangeQuestionViewModel QuestionChange { get; set; }

        public ChangeSecurityViewModel()
        {
            PasswordChange = new ChangePasswordViewModel();
            QuestionChange = new ChangeQuestionViewModel();
        }
    }
}
