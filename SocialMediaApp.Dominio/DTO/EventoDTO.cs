namespace SocialMediaApp.Dominio.DTO
{
    public class EventoDTO
    {
        public int EventoId { get; set; }

        public int? UsuarioId { get; set; }

        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateTime? FechaEvento { get; set; }

        public string? Ubicacion { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }
}