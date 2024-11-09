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

        public RepositorioNotificaciones(SocialMediaDBContext context)
        {
            _context = context;
        }

        // Agrega una nueva notificación
        public async Task AgregarNotificacionAsync(Notificacione notificacione)
        {
            _context.Notificaciones.Add(notificacione);
            await _context.SaveChangesAsync();

        }

        // Marca una notificación como leída
        public async Task MarcarNotificacionComoLeidaAsync(int Notificacionid)
        {
            var notificacion = await _context.Notificaciones.FindAsync(Notificacionid);
            if (notificacion != null)
            {
                notificacion.EsLeida = true;
                await _context.SaveChangesAsync();
            }
        }

        // Obtiene todas las notificaciones de un usuario específico
        public async Task<IEnumerable<Notificacione?>> ObtenerNotificacionesParaUsuarioAsync(int Usuarioid)
        {
            return await _context.Notificaciones
                .Where(n => n.UsuarioId == Usuarioid)
                .ToListAsync();
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

        // Modifica una notificación existente
        public async Task ModificarNotificacionAsync(Notificacione notificacion)
        {
            var notificacionExistente = await _context.Notificaciones.FindAsync(notificacion.NotificacionId);
            if (notificacionExistente != null)
            {
                // Actualiza las propiedades de la notificación existente
                notificacionExistente.Titulo = notificacion.Titulo;
                notificacionExistente.Mensaje = notificacion.Mensaje;
                notificacionExistente.Fecha = notificacion.Fecha;
                notificacionExistente.EsLeida = notificacion.EsLeida;

                _context.Notificaciones.Update(notificacionExistente);
                await _context.SaveChangesAsync();
            }
        }
    }

}
