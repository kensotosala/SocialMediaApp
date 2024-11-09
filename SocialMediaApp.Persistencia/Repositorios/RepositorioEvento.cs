using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class RepositorioEvento:IEvento
    {
        private readonly SocialMediaDBContext _context;

        public RepositorioEvento(SocialMediaDBContext context)
        {
            _context = context;
        }

        // Obtiene eventos creados por el usuario o eventos en los que está invitado
        public async Task<IEnumerable<Evento>> ObtenerEventosParaUsuarioAsync(int UsuarioId)
        {
            return await _context.Eventos
                .Include(e => e.InvitadosEventos)
                .Where(e => e.UsuarioId == UsuarioId || e.InvitadosEventos.Any(i => i.UsuarioId == UsuarioId))
                .ToListAsync();
        }

        // Agrega un nuevo evento
        public async Task AgregarEventoAsync(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
        }

        // Invita a un usuario a un evento
        public async Task InvitarUsuarioAsync(int eventoId, int usuarioId)
        {
            var invitado = new InvitadosEvento
            {
                EventoId = eventoId,
                UsuarioId = usuarioId,
                Confirmacion = "Pendiente"
            };
            _context.InvitadosEventos.Add(invitado);
            await _context.SaveChangesAsync();
        }

        // Confirma o rechaza la asistencia del usuario al evento
        public async Task ConfirmarAsistenciaAsync(int eventoId, int usuarioId, string confirmacion)
        {
            var invitado = await _context.InvitadosEventos
                .FirstOrDefaultAsync(i => i.EventoId == eventoId && i.UsuarioId == usuarioId);

            if (invitado != null)
            {
                invitado.Confirmacion = confirmacion;
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

        // Modifica un evento existente
        public async Task ModificarEventoAsync(Evento evento)
        {
            var eventoExistente = await _context.Eventos.FindAsync(evento.EventoId);
            if (eventoExistente != null)
            {
                // Actualizamos los datos del evento existente con los nuevos valores
                eventoExistente.Titulo = evento.Titulo;
                eventoExistente.Descripcion = evento.Descripcion;
                eventoExistente.Ubicacion = evento.Ubicacion;
                eventoExistente.UsuarioId = evento.UsuarioId;

                _context.Eventos.Update(eventoExistente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
