using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IComentarios
    {
        // Inserta un nueva comentario
        public Task<List<Comentario>> obtenerTodo();
        // Inserta un nueva comentario
        public Task<int> insertarComentario(Comentario comentario);
        // Modificar un comentario
        public Task<int> modificarComentario(Comentario comentario);
        // Modificar un comentario
        public Task<int> eliminarComentario(Comentario comentario);
    }
}
