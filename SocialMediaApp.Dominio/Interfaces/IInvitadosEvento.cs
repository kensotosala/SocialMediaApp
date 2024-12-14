using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IInvitadosEvento
    {
        Task InvitarUsuarioAsync(int eventoId, int usuarioId);

        Task<List<InvitadosEvento>> ListarInvitadosAEvento();

        Task<List<InvitadosEvento>> ObtenerInvitadosPorEventoAsync(int eventoId);

        Task<InvitadosEvento?> ObtenerInvitacionPorIdAsync(int invitacionId);

        Task EliminarInvitacionAsync(int invitacionId);

        Task ModificarEstadoInvitacionAsync(int invitacionId, string nuevoEstado);
    }
}