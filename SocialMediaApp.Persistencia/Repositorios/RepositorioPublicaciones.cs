using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class RepositorioPublicaciones : IPublicaciones
    {
        private readonly SocialMediaDBContext _context;

        public RepositorioPublicaciones(SocialMediaDBContext context)
        {
            _context = context;
        }

        public Task<int> eliminarPublicacion(Publicacione publicacion)
        {
            _context.Publicaciones.Remove(publicacion);
            return _context.SaveChangesAsync();
        }

        public Task<int> insertarPublicacion(Publicacione publicaciones)
        {
            _context.Add(publicaciones);
            return _context.SaveChangesAsync();
        }

        public Task<int> ModificarPublicacion(Publicacione publicacion)
        {
            _context.Publicaciones.Update(publicacion);
            return _context.SaveChangesAsync();
        }

        public Task<List<Publicacione>> obtenerComentariosxPublicacionId(int PublicacioID)
        {
            return _context.Publicaciones
                .Where(p => p.PublicacionId == PublicacioID)
                .Include(c => c.Comentarios).ToListAsync();
        }

        public Task<Publicacione?> obtenerPublicacionxID(int publicacionID)
        {
            return _context.Publicaciones
                .Include(c => c.Comentarios)
                .FirstOrDefaultAsync(p => p.PublicacionId == publicacionID);
        }

        public Task<List<Publicacione>> obtenerTodo()
        {
            return _context.Publicaciones.ToListAsync();
        }
    }

}
