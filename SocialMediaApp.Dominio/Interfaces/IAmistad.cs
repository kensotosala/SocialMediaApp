using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IAmistad
    {
        public Task<List<Amistade>> ObtenerListaAmistadesAsync();

        public Task<List<Amistade>> ObtenerListaAmistadporNombreAsyn();

    }
}
