using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Dominio.DTO
{
    public class AmistadDTO
    {
        public int AmistadId { get; set; }

        public int? UsuarioId { get; set; }

        public int? AmigoId { get; set; }

        public string? Estado { get; set; }

        public DateTime? FechaSolicitud { get; set; }

        public DateTime? FechaAceptacion { get; set; }

        public virtual Usuario? Amigo { get; set; }

        public virtual Usuario? Usuario { get; set; }

    }
}
