using SocialMediaApp.Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.Interfaces
{
    
        public interface IInvitadosEvento
        {
            
            Task InvitarUsuarioAsync(int eventoId, int usuarioId);

            // Obtiene la lista de todos los invitados a un evento
            Task<List<InvitadosEvento>> ObtenerInvitadosPorEventoAsync(int eventoId);

            // Obtiene la información de un invitado por ID de invitación
            Task<InvitadosEvento?> ObtenerInvitacionPorIdAsync(int invitacionId);

            // Elimina una invitación por ID
            Task EliminarInvitacionAsync(int invitacionId);

            // Modifica el estado de una invitación
            Task ModificarEstadoInvitacionAsync(int invitacionId, string nuevoEstado);

        }
        
    
}
