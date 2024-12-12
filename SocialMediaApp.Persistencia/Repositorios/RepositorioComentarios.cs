using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class RepositorioComentarios : IComentarios
    {
        private readonly SocialMediaDBContext _context;

        public RepositorioComentarios(SocialMediaDBContext context)
        {
            _context = context;
        }

        public Task<int> eliminarComentario(Comentario comentario)
        {
            _context.Remove(comentario);
            return _context.SaveChangesAsync();
        }

        public Task<int> insertarComentario(Comentario comentario)
        {
            _context.Add(comentario);
            return _context.SaveChangesAsync();
        }

        public Task<int> modificarComentario(Comentario comentario)
        {
            _context.Update(comentario);
            return _context.SaveChangesAsync();
        }

        public Task<List<Comentario>> obtenerTodo()
        {
            return _context.Comentarios.ToListAsync();
        }

    }
}
