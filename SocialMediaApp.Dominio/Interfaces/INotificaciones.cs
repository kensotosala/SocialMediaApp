using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface INotificaciones
    {
        // Obtiene todas las notificaciones
        Task<List<Notificacione>> obtenerTodo();

        // Inserta una nueva notificación
        Task<int> insertar(Notificacione notificacion);

        // Obtiene una notificación específica por ID
        Task<Notificacione?> ObtenerPorIdAsync(int notificacionId);

        // Marca una notificación como leída
        Task MarcarNotificacionComoLeidaAsync(int notificacionId);

        // Modifica una notificación existente
        Task ModificarNotificacionAsync(Notificacione notificacion);

        // Elimina una notificación por ID
        Task EliminarNotificacionAsync(int notificacionId);

        // Obtiene todas las notificaciones de un usuario específico
        Task<IEnumerable<Notificacione?>> ObtenerNotificacionesParaUsuarioAsync(int usuarioId);
    }
}
