using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.ViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Se requiere un nombre de usuario.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Se quiere una contraseña.")]
        public string Contraseña { get; set; }
    }
}
