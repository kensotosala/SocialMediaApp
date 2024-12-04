using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class RepositorioAmistad : IAmistad
    {
        private readonly SocialMediaDBContext _context;

        public RepositorioAmistad(SocialMediaDBContext context)
        {
            _context = context;
        }

        // Obtiene un evento específico
        public Task<List<Amistade>> ObtenerListaAmistadesAsync()
        {
            return _context.Amistades.ToListAsync();
        }

        public Task<List<Amistade>> ObtenerListaAmistadporNombreAsyn()
        {
            return _context.Amistades
            .Include(a => a.Usuario)
            .Include(a => a.Amigo)     // Carga el amigo relacionado
            .ToListAsync();
        }
    }
}
