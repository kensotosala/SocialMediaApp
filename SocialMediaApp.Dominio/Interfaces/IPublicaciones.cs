using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IPublicaciones
    {
        // Obtiene todas las publicaciones
        public Task<List<Publicacione>> obtenerTodo();

        // Inserta una nueva publicacion
        public Task<int> insertarPublicacion(Publicacione publicaciones);
        // Inserta una nueva publicacion
        public Task<int> eliminarPublicacion(Publicacione publicacion);

        // Modificar una nueva publicaciones
        public Task<int> ModificarPublicacion(Publicacione publicacion);

        // Obtener publicacion por ID
        public Task<Publicacione?> obtenerPublicacionxID(int publicacionID);

        // obtener los comentarios por publicacionID
        public Task<List<Publicacione>> obtenerComentariosxPublicacionId(int PublicacioID);

    }
}
