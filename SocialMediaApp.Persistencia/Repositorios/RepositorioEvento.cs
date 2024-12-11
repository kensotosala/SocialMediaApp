﻿using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        public async Task<Evento> ObtenerEventosParaUsuarioAsync(int eventoID)
        {
            return await _context.Eventos
            .Include(e => e.InvitadosEventos)
              .FirstOrDefaultAsync(e => e.EventoId == eventoID);
        }

        // Obtiene un evento específico
        public async Task<IEnumerable<Evento?>> ObtenerEventoAsync()
        {
            return await _context.Eventos
                .Include(e => e.InvitadosEventos)
                .ToListAsync();
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