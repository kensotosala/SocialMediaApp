using SocialMediaApp.Persistencia.Data;
namespace SocialMediaApp.Dominio.Interfaces

{
    public interface IBuscar_Usuario

    {
       public Task<List<Usuario>> ObtenerListaUsuariosAsync();

        public Task<Usuario?> ObtenerUsuarioXNombreAsync(string NombreUsuario);

    }
}


