using SocialMediaApp.Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface INotificaciones
    {
        public Task<IEnumerable<Notificacione?>> ObtenerNotificacionesParaUsuarioAsync(int Usuarioid);
        public Task AgregarNotificacionAsync(Notificacione notificacione);
        public Task MarcarNotificacionComoLeidaAsync(int Notificacionid);

    }
}
