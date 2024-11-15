using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class RepositorioEvento : IEvento
    {
        private readonly SocialMediaDBContext _context;

        public RepositorioEvento(SocialMediaDBContext context)
        {
            _context = context;
        }

        // Obtiene eventos creados por el usuario o eventos en los que está invitado
        public async Task<IEnumerable<Evento>> ObtenerEventosParaUsuarioAsync(int usuarioId)
        {
            return await _context.Eventos
                .Include(e => e.InvitadosEventos)
                .Where(e => e.UsuarioId == usuarioId || e.InvitadosEventos.Any(i => i.UsuarioId == usuarioId))
                .ToListAsync();
        }

        // Obtiene un evento específico
        public async Task<Evento?> ObtenerEventoAsync(int eventoId)
        {
            return await _context.Eventos
                .Include(e => e.InvitadosEventos)
                .FirstOrDefaultAsync(e => e.EventoId == eventoId);
        }

        // Agrega un nuevo evento
        public async Task AgregarEventoAsync(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
        }

        // Modifica un evento existente
        public async Task ModificarEventoAsync(Evento evento)
        {
            var eventoExistente = await _context.Eventos.FindAsync(evento.EventoId);
            if (eventoExistente != null)
            {
                // Actualiza los datos del evento existente con los nuevos valores
                eventoExistente.Titulo = evento.Titulo;
                eventoExistente.Descripcion = evento.Descripcion;
                eventoExistente.Ubicacion = evento.Ubicacion;
                eventoExistente.FechaEvento = evento.FechaEvento;
                eventoExistente.UsuarioId = evento.UsuarioId;

                _context.Eventos.Update(eventoExistente);
                await _context.SaveChangesAsync();
            }
        }

        // Elimina un evento existente
        public async Task EliminarEventoAsync(int eventoId)
        {
            var evento = await _context.Eventos.FindAsync(eventoId);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
