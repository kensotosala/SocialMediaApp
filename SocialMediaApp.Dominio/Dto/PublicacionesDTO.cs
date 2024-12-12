namespace SocialMediaApp.Dominio.Dto
{
    public class PublicacionesDTO
    {
        public int publicacionID {  get; set; }
        public int usuarioID { get; set; }
        public string publicacion { get; set; } = string.Empty;
        public string imagen { get; set; } = string.Empty;
        public string enlace { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } 

    }
}
