using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class RepositorioNotificaciones : INotificaciones
    {
        private readonly SocialMediaDBContext _context;

        public RepositorioNotificaciones(SocialMediaDBContext context)
        {
            _context = context;
        }

        // Agrega una nueva notificación
        public async Task<int> insertar(Notificacione notificacion)
        {
            _context.Add(notificacion);
            return await _context.SaveChangesAsync();
        }

        // Obtiene una notificación específica por ID
        public async Task<Notificacione?> ObtenerPorIdAsync(int notificacionId)
        {
            return await _context.Notificaciones.Include(n => n.Usuario)
                                                .FirstOrDefaultAsync(n => n.NotificacionId == notificacionId);
        }

        // Obtiene todas las notificaciones
        public Task<List<Notificacione>> obtenerTodo()
        {
            return _context.Notificaciones.Include(n => n.Usuario).ToListAsync();
        }

        // Marca una notificación como leída
        public async Task MarcarNotificacionComoLeidaAsync(int notificacionId)
        {
            var notificacion = await _context.Notificaciones.FindAsync(notificacionId);
            if (notificacion != null)
            {
                notificacion.EsLeida = true;
                await _context.SaveChangesAsync();
            }
        }

        // Modifica una notificación existente
        public async Task ModificarNotificacionAsync(Notificacione notificacion)
        {
            var notificacionExistente = await _context.Notificaciones.FindAsync(notificacion.NotificacionId);
            if (notificacionExistente != null)
            {
                // Actualiza las propiedades de la notificación existente
                notificacionExistente.Tipo = notificacion.Tipo;
                notificacionExistente.Descripcion = notificacion.Descripcion;
                notificacionExistente.Fecha = notificacion.Fecha;
                notificacionExistente.EsLeida = notificacion.EsLeida;

                _context.Notificaciones.Update(notificacionExistente);
                await _context.SaveChangesAsync();
            }
        }

        // Elimina una notificación
        public async Task EliminarNotificacionAsync(int notificacionId)
        {
            var notificacion = await _context.Notificaciones.FindAsync(notificacionId);
            if (notificacion != null)
            {
                _context.Notificaciones.Remove(notificacion);
                await _context.SaveChangesAsync();
            }
        }

        // Obtiene todas las notificaciones de un usuario específico
        public async Task<IEnumerable<Notificacione?>> ObtenerNotificacionesParaUsuarioAsync(int usuarioId)
        {
            return await _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId)
                .ToListAsync();
        }
    }
}
