using System.Text.Json.Serialization;

namespace SocialMediaApp.Dominio.DTO
{
    public class CrearEventoDTO
    {
        [JsonPropertyName("usuarioId")]
        public int? UsuarioId { get; set; }

        [JsonPropertyName("titulo")]
        public string Titulo { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("fechaEvento")]
        public DateTime FechaEvento { get; set; }

        [JsonPropertyName("ubicacion")]
        public string Ubicacion { get; set; }

        [JsonPropertyName("usuarioIds")]
        public List<int> UsuarioIds { get; set; } = new List<int>();
    }
}