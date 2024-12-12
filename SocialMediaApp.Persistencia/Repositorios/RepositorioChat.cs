using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class RepositorioChat : IChat
    {
        private readonly SocialMediaDBContext _context;

        public RepositorioChat(SocialMediaDBContext context)
        {
            _context = context;
        }

        //Enviar un mensaje de un usuario a otro
        public async Task EnviarMensajeAsync(int emisorId, int receptorId, string mensajeTexto)
        {
            var nuevoMensaje = new Mensaje
            {
                EmisorId = emisorId,
                ReceptorId = receptorId,
                MensajeTexto = mensajeTexto,
                FechaEnvio = DateTime.Now,
                EsLeido = false
            };

            _context.Mensajes.Add(nuevoMensaje);
            await _context.SaveChangesAsync();
        }

        //Obtener mensajes entre dos usuarios
        public async Task<IEnumerable<Mensaje>> ObtenerMensajesEntreUsuariosAsync(int usuario1Id, int usuario2Id)
        {
            return await _context.Mensajes
                .Where(m =>
                    (m.EmisorId == usuario1Id && m.ReceptorId == usuario2Id) ||
                    (m.EmisorId == usuario2Id && m.ReceptorId == usuario1Id))
                .OrderBy(m => m.FechaEnvio).ToListAsync();
        }

        //Obtener mensajes no leidos para un usuario
        public async Task<IEnumerable<Mensaje>> ObtenerMensajesNoLeidosAsync(int receptorId)
        {
            return await _context.Mensajes
                .Where(m => m.ReceptorId == receptorId && (m.EsLeido == false || m.EsLeido == null))
                .OrderBy(m => m.FechaEnvio).ToListAsync();
        }

        //Marcar mensajes como leidos
        public async Task MarcarMensajesComoLeidosAsync(int receptorId, int emisorId)
        {
            var mensajesNoLeidos = await _context.Mensajes
                .Where(m => m.ReceptorId == receptorId && m.EmisorId == emisorId && (m.EsLeido == false || m.EsLeido == null))
                .ToListAsync();

            foreach (var mensaje in mensajesNoLeidos) 
            {
                mensaje.EsLeido = true;
            }
        }
    }
}
