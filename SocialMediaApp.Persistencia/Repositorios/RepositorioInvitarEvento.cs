using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class RepositorioInvitarEvento : IInvitadosEvento
    {
        private readonly SocialMediaDBContext _context;

        public RepositorioInvitarEvento(SocialMediaDBContext context)
        {
            _context = context;
        }

        public async Task InvitarUsuarioAsync(int eventoId, int usuarioId)
        {
            // Validar si el evento existe
            var evento = await _context.Eventos.FindAsync(eventoId);
            if (evento == null)
            {
                throw new Exception("El evento no existe.");
            }

            // Validar si el usuario ya está invitado
            var invitacionExistente = await _context.InvitadosEventos
                .FirstOrDefaultAsync(i => i.EventoId == eventoId && i.UsuarioId == usuarioId);

            if (invitacionExistente != null)
            {
                throw new Exception("El usuario ya está invitado a este evento.");
            }

            // Crear la nueva invitación
            var nuevaInvitacion = new InvitadosEvento
            {
                EventoId = eventoId,
                UsuarioId = usuarioId,
                Confirmacion = "Pendiente" // Estado inicial
            };

            // Agregar la invitación a la base de datos
            _context.InvitadosEventos.Add(nuevaInvitacion);
            await _context.SaveChangesAsync();
        }

        public async Task<List<InvitadosEvento>> ObtenerInvitadosPorEventoAsync(int eventoId)
        {
            return await _context.InvitadosEventos
                .Where(i => i.EventoId == eventoId)
                .Include(i => i.Usuario) // Incluye información del usuario invitado
                .ToListAsync();
        }

        public async Task<InvitadosEvento?> ObtenerInvitacionPorIdAsync(int invitacionId)
        {
            return await _context.InvitadosEventos
                .Include(i => i.Usuario) // Incluye información del usuario invitado
                .FirstOrDefaultAsync(i => i.InvitadoId == invitacionId);
        }

        public async Task EliminarInvitacionAsync(int invitacionId)
        {
            var invitacion = await _context.InvitadosEventos.FindAsync(invitacionId);
            if (invitacion != null)
            {
                _context.InvitadosEventos.Remove(invitacion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ModificarEstadoInvitacionAsync(int invitacionId, string nuevoEstado)
        {
            var invitacion = await _context.InvitadosEventos.FindAsync(invitacionId);
            if (invitacion != null)
            {
                invitacion.Confirmacion = nuevoEstado;
                await _context.SaveChangesAsync();
            }
        }
    }
}
