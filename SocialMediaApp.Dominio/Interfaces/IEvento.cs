using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IEvento
    {
        Task<Evento> ObtenerEventosParaUsuarioAsync(int eventoID);

        Task<IEnumerable<Evento?>> ObtenerEventoAsync();

        Task AgregarEventoAsync(Evento evento);

        Task ModificarEventoAsync(Evento evento);

        Task EliminarEventoAsync(int eventoId);

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