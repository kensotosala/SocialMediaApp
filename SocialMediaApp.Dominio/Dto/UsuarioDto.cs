namespace SocialMediaApp.Persistencia.Data
{
    public class UsuarioDto
    {
        public string NombreUsuario { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Nombre { get; set; } = null!;

        public string? Apellido { get; set; } = null!;

        public string? FotoPerfil { get; set; }

        public string? Biografia { get; set; }

        public string? Ubicacion { get; set; }

        public string? Intereses { get; set; }

        public bool? EsPremium { get; set; }

        public bool AutenticacionExternal { get; set; }
    }
}
