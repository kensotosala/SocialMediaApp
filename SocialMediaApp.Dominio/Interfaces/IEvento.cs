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
        

    }
}
