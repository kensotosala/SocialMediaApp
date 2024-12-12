using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IChat
    {
        Task EnviarMensajeAsync(int emisorId, int receptorId, string mensajeTexto);
        Task<IEnumerable<Mensaje>> ObtenerMensajesEntreUsuariosAsync(int usuario1Id, int usuario2Id);
        Task<IEnumerable<Mensaje>> ObtenerMensajesNoLeidosAsync(int receptorId);
        Task MarcarMensajesComoLeidosAsync(int receptorId, int emisorId);

    }
}
