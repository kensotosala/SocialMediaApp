using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace SocialMediaApp.Persistencia.Data
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public string Pregunta { get; set; }
        public string Respuesta { get; set; }
    }

    public class ChangeQuestionRequest
    {
        public string Pregunta { get; set; }
        public string Respuesta { get; set; }
        public string Username { get; set; }
    }

    public class Profile
    {
        public string NombreUsuario{ get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? Biografia { get; set; }
        public string? Ubicacion { get; set; }
        public string Email { get; set; }

    }
}
