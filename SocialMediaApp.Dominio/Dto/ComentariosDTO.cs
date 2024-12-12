namespace SocialMediaApp.Dominio.Dto
{
    public class ComentariosDTO
    {
        public int comentarioID {  get; set; }
        public int publicacionID { get; set; }
        public int usuarioID { get; set; }
        public string comentario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }

    }
}
