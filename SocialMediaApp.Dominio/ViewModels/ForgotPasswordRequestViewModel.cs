using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.ViewModels
{
    public class ForgotPasswordRequestViewModel
    {
        [Required(ErrorMessage = "Se debe ingresar un nombre de usuario.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Se debe ingresar un correo electrónico.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Se quiere una pregunta de seguridad,.")]
        public string Pregunta { get; set; } = null!;

        [Required(ErrorMessage = "Se quiere una respuesta.")]
        public string Respuesta { get; set; } = null!;
    }
}
