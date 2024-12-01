namespace SocialMediaApp.Dominio.DTO
{
    public class NotificacionDTO
    {
        public int NotificacionId { get; set; }

        public int? UsuarioId { get; set; }

        public string? Tipo { get; set; }

        public string? Descripcion { get; set; }

        public DateTime? Fecha { get; set; }

        public bool? EsLeida { get; set; }
    }
}
