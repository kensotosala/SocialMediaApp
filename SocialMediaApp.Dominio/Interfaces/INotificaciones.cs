using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface INotificaciones
    {
        public Task<List<Notificacione>> obtenerTodo();

        //public Task AgregarNotificacionAsync(Notificacione notificacione);

        //public Task MarcarNotificacionComoLeidaAsync(int Notificacionid);

        public Task<int> insertar(Notificacione notifacion);
    }
}