using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class RepositorioNotificanciones : INotificaciones
    {
        private readonly SocialMediaDBContext _context;

        public RepositorioNotificanciones(SocialMediaDBContext context)
        {
            _context = context;
        }

        // Agrega una nueva notificación
        public async Task AgregarNotificacionAsync(Notificacione notificacione)
        {
            _context.Notificacione.Add(notificacione);
            await _context.SaveChangesAsync();
        }

        // Marca una notificación como leída
        public async Task MarcarNotificacionComoLeidaAsync(int Notificacionid)
        {
            var notificacion = await _context.Notificacione.FindAsync(Notificacionid);
            if (notificacion != null)
            {
                notificacion.EsLeida = true;
                await _context.SaveChangesAsync();
            }
        }

        // Obtiene todas las notificaciones de un usuario específico
        public async Task<IEnumerable<Notificacione?>> ObtenerNotificacionesParaUsuarioAsync(int Usuarioid)
        {
            return await _context.Notificacione
                .Where(n => n.UsuarioId == Usuarioid)
                .ToListAsync();
        }

        // Elimina una notificación
        public async Task EliminarNotificacionAsync(int notificacionId)
        {
            var notificacion = await _context.Notificacione.FindAsync(notificacionId);
            if (notificacion != null)
            {
                _context.Notificacione.Remove(notificacion);
                await _context.SaveChangesAsync();
            }
        }

        // Modifica una notificación existente
        public async Task ModificarNotificacionAsync(Notificacione notificacion)
        {
            var notificacionExistente = await _context.Notificacione.FindAsync(notificacion.NotificacionId);
            if (notificacionExistente != null)
            {
                // Actualiza las propiedades de la notificación existente

                notificacionExistente.Fecha = notificacion.Fecha;
                notificacionExistente.EsLeida = notificacion.EsLeida;

                _context.Notificacione.Update(notificacionExistente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> insertar(Notificacione notifacion)
        {
            _context.Add(notifacion);
            var res = _context.SaveChangesAsync();
            return await res;
        }

        public Task<List<Notificacione>> obtenerTodo()
        {
            throw new NotImplementedException();
        }

        public Task<Notificacione?> ObtenerPorIdAsync(int notificacionId)
        {
            throw new NotImplementedException();
        }
    }
}