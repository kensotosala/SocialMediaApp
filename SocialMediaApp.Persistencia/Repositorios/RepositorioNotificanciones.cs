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
    public class RepositorioNotificanciones : INotificaciones
    {
        private readonly SocialMediaDBContext _context;

        public RepositorioNotificanciones(SocialMediaDBContext context)
        {
            _context = context;
        }

        public async Task AgregarNotificacionAsync(Notificacione notificacione)
        {
             _context.Notificaciones.Add(notificacione);
            await _context.SaveChangesAsync();

        }

        public async Task MarcarNotificacionComoLeidaAsync(int Notificacionid)
        {
            var notificacion = await _context.Notificaciones.FindAsync(Notificacionid);
            if (notificacion != null)
            {
                notificacion.EsLeida = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Notificacione?>> ObtenerNotificacionesParaUsuarioAsync(int Usuarioid)
        {
            return await _context.Notificaciones
        .Where(n => n.UsuarioId == Usuarioid)
        .ToListAsync();
        }
    }

       

       
    
}



