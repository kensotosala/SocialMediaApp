using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class Reaccione
{
    public int ReaccionId { get; set; }

    public int? PublicacionId { get; set; }

    public int? UsuarioId { get; set; }

    public string? TipoReaccion { get; set; }

    public virtual Publicacione? Publicacion { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
