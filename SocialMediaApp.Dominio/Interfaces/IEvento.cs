using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IEvento
    {
        Task<IEnumerable<Evento>> ObtenerEventosParaUsuarioAsync(int usuarioId);
        Task<Evento?> ObtenerEventoAsync(int eventoId);
        Task AgregarEventoAsync(Evento evento);
        Task ModificarEventoAsync(Evento evento);
        Task EliminarEventoAsync(int eventoId);
    }
}
