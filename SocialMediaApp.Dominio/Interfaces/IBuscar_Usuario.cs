using SocialMediaApp.Persistencia.Data;
<<<<<<< HEAD
namespace SocialMediaApp.Dominio.Interfaces

{
    public interface IBuscar_Usuario

    {
       public Task<List<Usuario>> ObtenerListaUsuariosAsync();

        public Task<Usuario?> ObtenerUsuarioXNombreAsync(string NombreUsuario);

    }
}


=======

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IBuscar_Usuario
    {
        public Task<List<Usuario>> ObtenerListaUsuariosAsync();

        public Task<Usuario?> ObtenerUsuarioXNombreAsync(string NombreUsuario);
        

    }
}
>>>>>>> pr/1
